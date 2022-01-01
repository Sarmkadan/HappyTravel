using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Extensions
{
    public static class InCompanyPermissionsExtensions
    {
        public static List<InCompanyPermissions> ToList(this InCompanyPermissions permissions)
        {
            return InCompanyPermissionValues
                .Where(v => permissions.HasFlag(v))
                .ToList();
        }


        private static readonly List<InCompanyPermissions> InCompanyPermissionValues = Enum.GetValues(typeof(InCompanyPermissions))
            .Cast<InCompanyPermissions>()
            .ToList();
    }
}