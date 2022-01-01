using System;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Data.Customers
{
    public class UserInvitation
    {
        public string CodeHash { get; set; }
        public string Data { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public bool IsAccepted { get; set; }
        public UserInvitationTypes InvitationType { get; set; }
    }
}