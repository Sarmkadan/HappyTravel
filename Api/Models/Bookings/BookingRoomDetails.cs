using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.General;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Bookings
{
    public readonly struct BookingRoomDetails
    {
        [JsonConstructor]
        public BookingRoomDetails(RoomTypes type, List<Pax> passengers, bool isExtraBedNeeded = false, bool isCotNeeded = false)
        {
            Type = type;
            Passengers = passengers ?? new List<Pax>();
            IsExtraBedNeeded = isExtraBedNeeded;
            IsCotNeededNeeded = isCotNeeded;
        }


        public List<Pax> Passengers { get; }

        public RoomTypes Type { get; }

        /// <summary>
        ///     Indicates if extra child bed needed.
        /// </summary>
        public bool IsExtraBedNeeded { get; }

        /// <summary>
        ///     Indicates if extra cot needed.
        /// </summary>
        public bool IsCotNeededNeeded { get; }
    }
}