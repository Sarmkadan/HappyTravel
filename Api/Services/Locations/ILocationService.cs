using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Availabilities;
using HappyTravel.Edo.Api.Models.Locations;
using Microsoft.AspNetCore.Mvc;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace HappyTravel.Edo.Api.Services.Locations
{
    public interface ILocationService
    {
        ValueTask<Result<Location, ProblemDetails>> Get(SearchLocation searchLocation, string languageCode);

        ValueTask<List<Country>> GetCountries(string query, string languageCode);

        ValueTask<Result<List<Prediction>, ProblemDetails>> GetPredictions(string query, string session, int customerId, string languageCode);

        ValueTask<List<Region>> GetRegions(string languageCode);

        Task Set(IEnumerable<Models.Locations.Location> locations);

        Task<DateTime> GetLastModifiedDate();
    }
}