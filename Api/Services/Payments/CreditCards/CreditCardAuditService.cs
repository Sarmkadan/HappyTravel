using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Users;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Payments;
using HappyTravel.EdoContracts.General.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Services.Payments.CreditCards
{
    public class CreditCardAuditService : ICreditCardAuditService
    {
        public CreditCardAuditService(EdoContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }


        public async Task Write<TEventData>(CreditCardEventType eventType, string maskedNumber, decimal amount, UserInfo user, TEventData eventData,
            string referenceCode, int customerId, Currencies currency)
        {
            var logEntry = new CreditCardAuditLogEntry()
            {
                Created = _dateTimeProvider.UtcNow(),
                Type = eventType,
                MaskedNumber = maskedNumber,
                Amount = amount,
                UserId = user.Id,
                UserType = user.Type,
                EventData = JsonConvert.SerializeObject(eventData),
                ReferenceCode = referenceCode,
                CustomerId = customerId,
                Currency = currency
            };

            _context.CreditCardAuditLogs.Add(logEntry);
            await _context.SaveChangesAsync();
        }


        private readonly EdoContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
    }
}