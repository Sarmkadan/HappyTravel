using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.Edo.Api.Models.Payments.CreditCards;

namespace HappyTravel.Edo.Api.Services.Payments.CreditCards
{
    public interface ICreditCardService
    {
        Task<List<CreditCardInfo>> Get(CustomerInfo customerInfo);

        Task<Result<CreditCardInfo>> Get(int cardId, CustomerInfo customerInfo);

        Task<Result<CreditCardInfo>> Save(SaveCreditCardRequest request, CustomerInfo customerInfo);

        Task<Result> Delete(int cardId, CustomerInfo customerInfo);

        TokenizationSettings GetTokenizationSettings();
    }
}