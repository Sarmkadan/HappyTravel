namespace HappyTravel.Edo.Api.Models.Management.AuditEvents
{
    public readonly struct AdministrationRegistrationEvent
    {
        public AdministrationRegistrationEvent(string email, int id, string invitationCode)
        {
            Email = email;
            Id = id;
            InvitationCode = invitationCode;
        }


        public string Email { get; }
        public int Id { get; }
        public string InvitationCode { get; }
    }
}