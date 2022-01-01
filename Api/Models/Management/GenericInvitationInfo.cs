namespace HappyTravel.Edo.Api.Models.Management
{
    public readonly struct GenericInvitationInfo
    {
        public GenericInvitationInfo(string email, string lastName, string firstName, string position, string title, int? companyId = null)
        {
            CompanyId = companyId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Title = title;
        }


        public int? CompanyId { get; }
        public string Email { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string Position { get; }
        public string Title { get; }
    }
}