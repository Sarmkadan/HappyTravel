using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.Edo.Api.Services.Customers;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.EdoContracts.General.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HappyTravel.Edo.Api.Services.Payments
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        public PaymentHistoryService(EdoContext edoContext, ICustomerContext customerContext, IPermissionChecker permissionChecker)
        {
            _edoContext = edoContext;
            _customerContext = customerContext;
            _permissionChecker = permissionChecker;
        }


        public async Task<Result<List<PaymentHistoryData>>> GetCustomerHistory(PaymentHistoryRequest paymentHistoryRequest, int companyId)
        {
            var validationResult = Validate(paymentHistoryRequest);
            if (validationResult.IsFailure)
                return Result.Fail<List<PaymentHistoryData>>(validationResult.Error);

            var customerInfoResult = await _customerContext.GetCustomerInfo();
            if (customerInfoResult.IsFailure)
                return Result.Fail<List<PaymentHistoryData>>(customerInfoResult.Error);

            var customerInfo = customerInfoResult.Value;

            var accountHistoryData = await _edoContext.PaymentAccounts.Where(a => a.CompanyId == companyId)
                    .Join(_edoContext.AccountBalanceAuditLogs
                            .Where(i => i.UserId == customerInfo.CustomerId)
                            .Where(i => i.UserType == UserTypes.Customer)
                            .Where(i => i.Created <= paymentHistoryRequest.ToDate &&
                                paymentHistoryRequest.FromDate <= i.Created),
                        pa => pa.Id,
                        bl => bl.AccountId,
                        (pa, bl) => new PaymentHistoryData(bl.Created,
                            bl.Amount,
                            JObject.Parse(bl.EventData),
                            pa.Currency.ToString(),
                            bl.UserId,
                            ToPaymentHistoryType(bl.Type),
                            PaymentMethods.BankTransfer))
                .ToListAsync();

            var cardHistoryData = await _edoContext.CreditCardAuditLogs
                .Where(i => i.CustomerId == customerInfo.CustomerId
                    && i.Created <= paymentHistoryRequest.ToDate
                    && paymentHistoryRequest.FromDate <= i.Created)
                .Select(a => new PaymentHistoryData(a.Created,
                    a.Amount, JObject.Parse(a.EventData),
                    a.Currency.ToString(),
                    a.UserId,
                    ToPaymentHistoryType(a.Type),
                    PaymentMethods.CreditCard))
                .ToListAsync();

            var result = accountHistoryData.Union(cardHistoryData).OrderByDescending(h => h.Created).ToList();
            return Result.Ok(result);
        }


        public async Task<Result<List<PaymentHistoryData>>> GetCompanyHistory(PaymentHistoryRequest paymentHistoryRequest, int companyId)
        {
            var validationResult = Validate(paymentHistoryRequest);
            if (validationResult.IsFailure)
                return Result.Fail<List<PaymentHistoryData>>(validationResult.Error);

            var customerInfoResult = await _customerContext.GetCustomerInfo();
            if (customerInfoResult.IsFailure)
                return Result.Fail<List<PaymentHistoryData>>(customerInfoResult.Error);

            var customerInfo = customerInfoResult.Value;

            var customerPermissionResult =
                await _permissionChecker.CheckInCompanyPermission(customerInfo.CustomerId, companyId, InCompanyPermissions.ViewCompanyAllPaymentHistory);
            if (customerPermissionResult.IsFailure)
                return Result.Fail<List<PaymentHistoryData>>(customerPermissionResult.Error);

            var accountHistoryData = await _edoContext.PaymentAccounts.Where(i => i.CompanyId == companyId)
                    .Join(_edoContext.AccountBalanceAuditLogs.Where(i => i.Created <= paymentHistoryRequest.ToDate &&
                            paymentHistoryRequest.FromDate <= i.Created),
                        pa => pa.Id,
                        bl => bl.AccountId,
                        (pa, bl) => new PaymentHistoryData(bl.Created,
                            bl.Amount, JObject.Parse(bl.EventData),
                            pa.Currency.ToString(),
                            bl.UserId,
                            ToPaymentHistoryType(bl.Type),
                            PaymentMethods.BankTransfer))
                .ToListAsync();

            var cardHistoryData = await _edoContext.CreditCardAuditLogs
                .Where(i => i.CustomerId == customerInfo.CustomerId
                    && i.Created <= paymentHistoryRequest.ToDate
                    && paymentHistoryRequest.FromDate <= i.Created)
                .Select(a => new PaymentHistoryData(a.Created,
                    a.Amount, JObject.Parse(a.EventData),
                    a.Currency.ToString(),
                    a.UserId,
                    ToPaymentHistoryType(a.Type),
                    PaymentMethods.CreditCard))
                    .ToListAsync();

            var result = accountHistoryData.Union(cardHistoryData).OrderByDescending(h => h.Created).ToList();
            return Result.Ok(result);
        }


        private Result Validate(PaymentHistoryRequest paymentHistoryRequest)
        {
            return GenericValidator<PaymentHistoryRequest>.Validate(setup =>
            {
                setup.RuleFor(i => i.ToDate)
                    .GreaterThanOrEqualTo(request => request.FromDate)
                    .WithMessage($"{nameof(paymentHistoryRequest.ToDate)} must be greater then {nameof(paymentHistoryRequest.FromDate)}");
                setup.RuleFor(i => (i.ToDate - i.FromDate).Days)
                    .LessThanOrEqualTo(MaxRequestDaysNumber)
                    .WithMessage(
                        $"Total days between {nameof(paymentHistoryRequest.FromDate)} and {nameof(paymentHistoryRequest.ToDate)} should be less or equal {MaxRequestDaysNumber}");
            }, paymentHistoryRequest);
        }


        private static PaymentHistoryType ToPaymentHistoryType(AccountEventType type)
        {
            switch (type)
            {
                case AccountEventType.None: return PaymentHistoryType.None;
                case AccountEventType.Add: return PaymentHistoryType.Add;
                case AccountEventType.Charge: return PaymentHistoryType.Charge;
                case AccountEventType.Authorize: return PaymentHistoryType.Authorize;
                case AccountEventType.Capture: return PaymentHistoryType.Capture;
                case AccountEventType.Void: return PaymentHistoryType.Void;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        private static PaymentHistoryType ToPaymentHistoryType(CreditCardEventType type)
        {
            switch (type)
            {
                case CreditCardEventType.Authorize: return PaymentHistoryType.Authorize;
                case CreditCardEventType.Capture: return PaymentHistoryType.Capture;
                case CreditCardEventType.Void: return PaymentHistoryType.None;
                case CreditCardEventType.None: return PaymentHistoryType.None;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        private const int MaxRequestDaysNumber = 3650;
        private readonly ICustomerContext _customerContext;


        private readonly EdoContext _edoContext;
        private readonly IPermissionChecker _permissionChecker;
    }
}