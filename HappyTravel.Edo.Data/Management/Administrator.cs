using System;

namespace HappyTravel.Edo.Data.Management
{
    public class Administrator
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string IdentityHash { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}