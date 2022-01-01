using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Api.Models.Accommodations
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccommodationFeatureTypes
    {
        None,
        Honeymooners,
        AdjoiningRooms,
        CommunicatingRooms,
        HighestFloorRoom,
        LowerFloorRoom,
        EarlyCheckIn,
        LateArrival,
        KingSizeBed,
        QueenSizeBed,
        DoubleBedForSoleUse,
        NoSmokingRoom,
        AirConditioner,
        ChampagneBottle,
        FruitBasket,
        WineBottle
    }
}