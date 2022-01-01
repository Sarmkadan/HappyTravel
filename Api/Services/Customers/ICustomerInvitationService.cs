using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICustomerInvitationService
    {
        Task<Result> SendInvitation(CustomerInvitationInfo invitationInfo);

        Task AcceptInvitation(string invitationCode);

        Task<Result<CustomerInvitationInfo>> GetPendingInvitation(string invitationCode);
    }
}