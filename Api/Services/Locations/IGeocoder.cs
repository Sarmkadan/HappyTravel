using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Availabilities;
using HappyTravel.Edo.Api.Models.Locations;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace HappyTravel.Edo.Api.Services.Locations
{
    public interface IGeoCoder
    {
        Task<Result<Location>> GetLocation(SearchLocation searchLocation, string languageCode);

        ValueTask<Result<List<Prediction>>> GetLocationPredictions(string query, string sessionId, int customerId, string languageCode);
    }
}