using System;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Data.Management
{
    public class ManagementAuditLogEntry
    {
        public int Id { get; set; }
        public ManagementEventType Type { get; set; }
        public DateTime Created { get; set; }
        public int AdministratorId { get; set; }
        public string EventData { get; set; }
    }
}