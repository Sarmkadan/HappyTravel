using System;
using System.Collections.Generic;
using GeoAPI.Geometries;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.GeoData.Enums;
using NetTopologySuite.Geometries;

namespace HappyTravel.Edo.Data.Locations
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public IPoint Coordinates { get; set; } = new Point(0, 0);
        public int DistanceInMeters { get; set; }
        public string Locality { get; set; }
        public string Name { get; set; }
        public PredictionSources Source { get; set; }
        public LocationTypes Type { get; set; }
        public List<DataProviders> DataProviders { get; set; }
        public DateTime Modified { get; set; }
    }
}