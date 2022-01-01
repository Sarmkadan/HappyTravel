namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public interface ISignedResponse
    {
        string Signature { get; }
    }
}