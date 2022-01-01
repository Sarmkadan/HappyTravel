using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICustomerPermissionManagementService
    {
        Task<Result> SetInCompanyPermissions(int customerId, List<InCompanyPermissions> permissions);
    }
}