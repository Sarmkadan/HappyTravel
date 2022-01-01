using System.Collections.Generic;

namespace HappyTravel.Edo.Data
{
    internal class EntityDbMappingInfo
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public Dictionary<string, string> PropertyMapping { get; set; }
    }
}