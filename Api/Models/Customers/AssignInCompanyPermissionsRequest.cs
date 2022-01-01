using System.Collections.Generic;
using HappyTravel.Edo.Common.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Customers
{
    public readonly struct AssignInCompanyPermissionsRequest
    {
        [JsonConstructor]
        public AssignInCompanyPermissionsRequest(List<InCompanyPermissions> permissions)
        {
            Permissions = permissions ?? new List<InCompanyPermissions>();
        }


        public List<InCompanyPermissions> Permissions { get; }
    }
}