using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum InCompanyPermissions
    {
        None = 1,
        EditCompanyInfo = 2,
        PermissionManagement = 4,
        CustomerInvitation = 8,
        AccommodationAvailabilitySearch = 16,
        AccommodationBooking = 32,
        ViewCompanyAllPaymentHistory = 64,
        // "All" permission level should be recalculated after adding new permission
        All = EditCompanyInfo | 
            PermissionManagement | 
            CustomerInvitation | 
            AccommodationAvailabilitySearch | 
            AccommodationBooking |
            ViewCompanyAllPaymentHistory // 126
    }
}