namespace HappyTravel.Edo.Api.Models.Bookings
{
    public readonly struct ProcessResult
    {
        public ProcessResult(string message)
        {
            Message = message;
        }


        /// <summary>
        ///     Process result message
        /// </summary>
        public string Message { get; }
    }
}