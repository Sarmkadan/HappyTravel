using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Management
{
    public readonly struct AdministratorInvitationInfo
    {
        [JsonConstructor]
        public AdministratorInvitationInfo(string email, string lastName, string firstName, string position, string title)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Position = position;
            Title = title;
        }


        public string Email { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string Position { get; }
        public string Title { get; }
    }
}