using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Branches;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Data.Customers;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICompanyService
    {
        Task<Result<Company>> Add(CompanyRegistrationInfo company);

        Task<Result> SetVerified(int companyId, string verifyReason);

        Task<Result<Branch>> AddBranch(int companyId, BranchInfo branch);

        Task<Branch> GetDefaultBranch(int companyId);
    }
}