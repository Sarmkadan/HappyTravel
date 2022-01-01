using System.Collections.Generic;

namespace HappyTravel.Edo.Api.Infrastructure.Options
{
    public class DataProviderOptions
    {
        public string Netstorming { get; set; }
        public string Illusions { get; set; }
        public List<Common.Enums.DataProviders> EnabledProviders { get; set; }
    }
}
