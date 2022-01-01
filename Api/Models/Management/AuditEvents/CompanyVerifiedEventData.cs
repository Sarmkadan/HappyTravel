namespace HappyTravel.Edo.Api.Models.Management.AuditEvents
{
    public readonly struct CompanyVerifiedAuditEventData
    {
        public CompanyVerifiedAuditEventData(int companyId, string reason)
        {
            CompanyId = companyId;
            Reason = reason;
        }


        public int CompanyId { get; }
        public string Reason { get; }
    }
}