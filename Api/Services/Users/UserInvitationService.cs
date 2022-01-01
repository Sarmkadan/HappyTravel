using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Infrastructure.Logging;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Customers;
using HappyTravel.MailSender;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Services.Users
{
    public class UserInvitationService : IUserInvitationService
    {
        public UserInvitationService(EdoContext context,
            IDateTimeProvider dateTimeProvider,
            IMailSender mailSender,
            ILogger<UserInvitationService> logger,
            IOptions<UserInvitationOptions> options)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _mailSender = mailSender;
            _logger = logger;
            _options = options.Value;
        }


        public async Task<Result> Send(string email,
            GenericInvitationInfo invitationInfo,
            string mailTemplateId,
            UserInvitationTypes invitationType)
        {
            var invitationCode = GenerateRandomCode();
            var addresseeEmail = email;

            return await SendInvitationMail()
                .OnSuccess(SaveInvitationData)
                .OnSuccess(LogInvitationCreated);


            string GenerateRandomCode()
            {
                using (var provider = new RNGCryptoServiceProvider())
                {
                    var byteArray = new byte[64];
                    provider.GetBytes(byteArray);
                    return Convert.ToBase64String(byteArray)
                        .Replace("/", string.Empty);
                }
            }


            async Task<Result> SendInvitationMail()
            {
                string companyName;
                if (invitationInfo.CompanyId is null)
                    companyName = "HappyTravelDotCom Travel & Tourism LLC";
                else
                    companyName = await _context.Companies
                        .Where(c => c.Id == invitationInfo.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefaultAsync();

                return await _mailSender.Send(mailTemplateId,
                    addresseeEmail,
                    new
                    {
                        companyName,
                        invitationCode,
                        userEmailAddress = addresseeEmail,
                        userName = $"{invitationInfo.FirstName} {invitationInfo.LastName}"
                    });
            }


            Task SaveInvitationData()
            {
                _context.UserInvitations.Add(new UserInvitation
                {
                    CodeHash = HashGenerator.ComputeSha256(invitationCode),
                    Created = _dateTimeProvider.UtcNow(),
                    Data = JsonConvert.SerializeObject(invitationInfo),
                    Email = addresseeEmail,
                    InvitationType = invitationType
                });

                return _context.SaveChangesAsync();
            }


            void LogInvitationCreated()
                => _logger.LogInvitationCreatedInformation(
                    $"Invitation for user {email} created");
        }


        public async Task Accept(string invitationCode)
        {
            var invitationMaybe = await GetInvitation(invitationCode);
            if (invitationMaybe.HasValue)
            {
                var invitation = invitationMaybe.Value;
                invitation.IsAccepted = true;
                _context.Update(invitation);
                await _context.SaveChangesAsync();
            }
        }


        public Task<Result<TInvitationData>> GetPendingInvitation<TInvitationData>(string invitationCode, UserInvitationTypes invitationType)
        {
            return GetInvitation(invitationCode).ToResult("Could not find invitation")
                .Ensure(IsNotAccepted, "Already accepted")
                .Ensure(HasCorrectType, "Invitation type mismatch")
                .Ensure(InvitationIsActual, "Invitation expired")
                .OnSuccess(GetInvitationData<TInvitationData>);

            bool IsNotAccepted(UserInvitation invitation) => !invitation.IsAccepted;

            bool HasCorrectType(UserInvitation invitation) => invitation.InvitationType == invitationType;

            bool InvitationIsActual(UserInvitation invitation) => invitation.Created + _options.InvitationExpirationPeriod > _dateTimeProvider.UtcNow();
        }


        private static TInvitationData GetInvitationData<TInvitationData>(UserInvitation invitation)
            => JsonConvert.DeserializeObject<TInvitationData>(invitation.Data);


        private async Task<Maybe<UserInvitation>> GetInvitation(string code)
        {
            var invitation = await _context.UserInvitations
                .SingleOrDefaultAsync(c => c.CodeHash == HashGenerator.ComputeSha256(code));

            return invitation ?? Maybe<UserInvitation>.None;
        }


        private readonly EdoContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<UserInvitationService> _logger;
        private readonly IMailSender _mailSender;
        private readonly UserInvitationOptions _options;
    }
}