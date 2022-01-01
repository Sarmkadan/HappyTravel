using CSharpFunctionalExtensions;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Models.Customers
{
    public readonly struct CustomerInfo
    {
        public CustomerInfo(int customerId, string firstName, string lastName, string email,
            string title, string position, int companyId, string companyName, int branchId, bool isMaster,
            InCompanyPermissions inCompanyPermissions)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Title = title;
            Position = position;
            CompanyId = companyId;
            CompanyName = companyName;
            BranchId = branchId;
            IsMaster = isMaster;
            InCompanyPermissions = inCompanyPermissions;
        }


        public void Deconstruct(out int customerId, out int companyId, out int branchId, out bool isMaster)
        {
            customerId = CustomerId;
            companyId = CompanyId;
            branchId = BranchId;
            isMaster = IsMaster;
        }


        public bool Equals(CustomerInfo other)
            => (CustomerId, CompanyId, BranchId, IsMaster)
                == (other.CustomerId, other.CompanyId, other.BranchId, other.IsMaster);


        public override bool Equals(object obj) => obj is CustomerInfo other && Equals(other);


        public override int GetHashCode() => (CustomerId, CompanyId, BranchId, IsMaster).GetHashCode();


        public int CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public int CompanyId { get; }
        public string CompanyName { get; }
        public int BranchId { get; }
        public bool IsMaster { get; }
        public InCompanyPermissions InCompanyPermissions { get; }
        public string Title { get; }
        public string Position { get; }
    }
}