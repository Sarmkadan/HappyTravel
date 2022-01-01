using System;

namespace HappyTravel.Edo.Api.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
}