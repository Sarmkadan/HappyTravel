using System.Threading.Tasks;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Api.Models.Markups
{
    public delegate ValueTask<decimal> AggregatedMarkupFunction(decimal supplierPrice, Currencies currency);
}