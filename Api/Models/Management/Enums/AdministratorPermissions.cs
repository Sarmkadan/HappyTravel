using System;

namespace HappyTravel.Edo.Api.Models.Management.Enums
{
    [Flags]
    public enum AdministratorPermissions
    {
        None = 1,
        CompanyVerification = 2,
        CreditLimitChange = 4,
        AccountReplenish = 8,
        MarkupManagement = 16
    }
}