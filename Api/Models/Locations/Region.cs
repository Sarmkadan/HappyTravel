namespace HappyTravel.Edo.Api.Models.Locations
{
    public readonly struct Region
    {
        public Region(int id, string name)
        {
            Id = id;
            Name = name;
        }


        /// <summary>
        ///     Region UN M.49 code.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     The dictionary of region names on supported languages.
        /// </summary>
        public string Name { get; }
    }
}