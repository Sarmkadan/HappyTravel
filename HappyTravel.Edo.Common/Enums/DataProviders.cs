namespace HappyTravel.Edo.Common.Enums
{
    // DO NOT ADD DESCRIPTIONS AND JSON SERIALIZING SETTINGS THERE
    // This enum should be serialized and deserialized as Int32 to hide real provider names from client.
    public enum DataProviders
    {
        Unknown = 0,
        Netstorming = 1,
        Illusions = 2,
        Direct = 3
    }
}