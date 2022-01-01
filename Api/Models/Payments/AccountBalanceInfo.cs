using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Api.Models.Payments
{
    /// <summary>
    ///     Account balance info
    /// </summary>
    public class AccountBalanceInfo
    {
        public AccountBalanceInfo(decimal balance, decimal creditLimit, Currencies currency)
        {
            Balance = balance;
            CreditLimit = creditLimit;
            Currency = currency;
        }


        /// <summary>
        ///     Balance
        /// </summary>
        public decimal Balance { get; }


        /// <summary>
        ///     Credit limit
        /// </summary>
        public decimal CreditLimit { get; }


        /// <summary>
        ///     Account currency
        /// </summary>
        public Currencies Currency { get; }
    }
}