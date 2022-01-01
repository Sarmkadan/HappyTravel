using HappyTravel.Edo.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HappyTravel.Edo.UnitTests.Infrastructure
{
    public static class MockEdoContext
    {
        public static Mock<EdoContext> Create()
        {
            return new Mock<EdoContext>(new DbContextOptions<EdoContext>());
        }
    }
}