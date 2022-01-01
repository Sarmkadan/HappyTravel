using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Data.Customers
{
    public class CustomerCompanyRelation
    {
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }
        public InCompanyPermissions InCompanyPermissions { get; set; }
        public int BranchId { get; set; }
        public CustomerCompanyRelationTypes Type { get; set; }


        public override bool Equals(object obj) => obj is CustomerCompanyRelation other && Equals(other);


        public bool Equals(CustomerCompanyRelation other)
            => Equals((CustomerId, CompanyId, InCompanyPermissions, BranchId, Type),
                (other.CustomerId, other.CompanyId, other.InCompanyPermissions, other.BranchId, other.Type));


        public override int GetHashCode() => (CustomerId, CompanyId, InCompanyPermissions, BranchId, Type).GetHashCode();
    }
}