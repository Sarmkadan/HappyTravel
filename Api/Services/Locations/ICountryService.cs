using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Models.Locations;

namespace HappyTravel.Edo.Api.Services.Locations
{
    public interface ICountryService
    {
        ValueTask<List<Country>> Get(string query, string languageCode);

        ValueTask<string> GetCode(string countryName, string languageCode);
    }
}