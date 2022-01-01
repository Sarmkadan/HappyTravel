using System;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Data.Payments
{
    public class PaymentAccount : IEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AuthorizedBalance { get; set; }
        public Currencies Currency { get; set; }
        public DateTime Created { get; set; }
    }
}