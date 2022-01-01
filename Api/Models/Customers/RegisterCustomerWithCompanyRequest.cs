using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Customers
{
    public struct RegisterCustomerWithCompanyRequest
    {
        [JsonConstructor]
        public RegisterCustomerWithCompanyRequest(CustomerRegistrationInfo customer, CompanyRegistrationInfo company)
        {
            Customer = customer;
            Company = company;
        }


        /// <summary>
        ///     Customer personal information.
        /// </summary>
        public CustomerRegistrationInfo Customer { get; }

        /// <summary>
        ///     Customer affiliated company information.
        /// </summary>
        public CompanyRegistrationInfo Company { get; }
    }
}