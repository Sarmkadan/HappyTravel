using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface IPermissionChecker
    {
        ValueTask<Result> CheckInCompanyPermission(CustomerInfo customer, InCompanyPermissions permission);

        ValueTask<Result> CheckInCompanyPermission(int customerId, int companyId, InCompanyPermissions permission);
    }
}