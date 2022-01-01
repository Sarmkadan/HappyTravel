using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HappyTravel.Edo.UnitTests.Infrastructure.DbSetMocks
{
    public class DbSetMockProvider
    {
        public static DbSet<T> GetDbSetMock<T>(IEnumerable<T> enumerable) where T : class
        {
            var mock = new Mock<DbSet<T>>();
            var queryable = enumerable.AsQueryable();

            mock.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            mock.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

            mock.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(queryable.Expression);

            mock.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(queryable.ElementType);

            mock.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => queryable.GetEnumerator());

            return mock.Object;
        }
    }
}