using System.Threading.Tasks;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Services.CodeProcessors
{
    public interface ITagProcessor
    {
        Task<string> GenerateReferenceCode(ServiceTypes serviceType, string destinationCode, string itineraryNumber);

        Task<string> GenerateNonSequentialReferenceCode(ServiceTypes serviceType, string destinationCode);

        Task<string> GenerateItn();

        bool TryGetItnFromReferenceCode(string referenceCode, out string itn);

        bool IsCodeValid(string referenceCode);
    }
}