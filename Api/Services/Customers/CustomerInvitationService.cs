using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Api.Services.Users;
using HappyTravel.Edo.Common.Enums;
using Microsoft.Extensions.Options;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public class CustomerInvitationService : ICustomerInvitationService
    {
        public CustomerInvitationService(ICustomerContext customerContext,
            IOptions<CustomerInvitationOptions> options,
            IUserInvitationService invitationService,
            IPermissionChecker permissionChecker)
        {
            _customerContext = customerContext;
            _invitationService = invitationService;
            _permissionChecker = permissionChecker;
            _options = options.Value;
        }


        public async Task<Result> SendInvitation(CustomerInvitationInfo invitationInfo)
        {
            var (_, isFailure, customerInfo, error) = await _customerContext.GetCustomerInfo();
            if (isFailure)
                return Result.Fail(error);

            if (customerInfo.CompanyId != invitationInfo.CompanyId)
                return Result.Fail("Invitations can be sent within a company only");

            var permissionCheckResult = await _permissionChecker.CheckInCompanyPermission(customerInfo, InCompanyPermissions.CustomerInvitation);
            if (permissionCheckResult.IsFailure)
                return permissionCheckResult;

            var payload = new GenericInvitationInfo(invitationInfo.Email, invitationInfo.RegistrationInfo.LastName, invitationInfo.RegistrationInfo.FirstName,
                invitationInfo.RegistrationInfo.Position, invitationInfo.RegistrationInfo.Title, invitationInfo.CompanyId);

            return await _invitationService.Send(invitationInfo.Email, payload,
                _options.MailTemplateId, UserInvitationTypes.Customer);
        }


        public Task AcceptInvitation(string invitationCode) => _invitationService.Accept(invitationCode);


        public Task<Result<CustomerInvitationInfo>> GetPendingInvitation(string invitationCode)
            => _invitationService.GetPendingInvitation<CustomerInvitationInfo>(invitationCode, UserInvitationTypes.Customer);


        private readonly ICustomerContext _customerContext;
        private readonly IUserInvitationService _invitationService;
        private readonly CustomerInvitationOptions _options;
        private readonly IPermissionChecker _permissionChecker;
    }
}