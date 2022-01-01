using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;

namespace HappyTravel.Edo.Api.Services.CodeProcessors
{
    internal class TagProcessor : ITagProcessor
    {
        public TagProcessor(EdoContext context)
        {
            _context = context;
        }


        public async Task<string> GenerateReferenceCode(ServiceTypes serviceType, string destinationCode, string itineraryNumber)
        {
            var currentNumber = await _context.GenerateNextItnMember(itineraryNumber);

            return string.Join(ReferenceCodeItemsSeparator, serviceType,
                destinationCode,
                itineraryNumber,
                currentNumber.ToString("D2"));
        }


        public bool TryGetItnFromReferenceCode(string referenceCode, out string itn)
        {
            itn = string.Empty;
            if (string.IsNullOrEmpty(referenceCode))
                return false;

            var referenceCodeItems = referenceCode.Split(ReferenceCodeItemsSeparator);
            //ReferenceCode can have 3 or 4 items, third is always itn
            if (referenceCodeItems.Length < 3)
                return false;

            itn = referenceCodeItems[2];
            return true;
        }


        public async Task<string> GenerateNonSequentialReferenceCode(ServiceTypes serviceType, string destinationCode)
        {
            var itineraryNumber = await GenerateItn();

            return string.Join(ReferenceCodeItemsSeparator, serviceType,
                destinationCode,
                itineraryNumber);
        }


        public async Task<string> GenerateItn()
        {
            var hash = string.Empty;
            var quotient = await _context.GetNextItineraryNumber();
            do
            {
                quotient = Math.DivRem(quotient, ItnNumeralSystemBase, out var remainder);
                hash += ItnNumeralSystemTable[remainder];
            } while (quotient != 0);

            var itn = hash.PadLeft(6, '0');
            await _context.RegisterItn(itn);

            return itn;
        }


        public bool IsCodeValid(string referenceCode)
        {
            return referenceCode.Length <= MaxReferenceCodeLength &&
                AvailableServiceTypes.Any(st => referenceCode.StartsWith(st.ToString(), StringComparison.OrdinalIgnoreCase));
        }


        private const string ReferenceCodeItemsSeparator = "-";
        private const long ItnNumeralSystemBase = 36;
        private const int MaxReferenceCodeLength = 22;

        private static readonly Dictionary<long, string> ItnNumeralSystemTable = new Dictionary<long, string>
        {
            {0, "0"},
            {1, "1"},
            {2, "2"},
            {3, "3"},
            {4, "4"},
            {5, "5"},
            {6, "6"},
            {7, "7"},
            {8, "8"},
            {9, "9"},
            {10, "A"},
            {11, "B"},
            {12, "C"},
            {13, "D"},
            {14, "E"},
            {15, "F"},
            {16, "G"},
            {17, "H"},
            {18, "I"},
            {19, "J"},
            {20, "K"},
            {21, "L"},
            {22, "M"},
            {23, "N"},
            {24, "O"},
            {25, "P"},
            {26, "Q"},
            {27, "R"},
            {28, "S"},
            {29, "T"},
            {30, "U"},
            {31, "V"},
            {32, "W"},
            {33, "X"},
            {34, "Y"},
            {35, "Z"}
        };

        private static readonly ServiceTypes[] AvailableServiceTypes = Enum
            .GetValues(typeof(ServiceTypes))
            .OfType<ServiceTypes>()
            .ToArray();

        private readonly EdoContext _context;
    }
}