using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Data;

namespace HappyTravel.Edo.Api.Infrastructure.FunctionalExtensions
{
    public static class ResultTransactionExtensions
    {
        public static Task<Result<T>> OnSuccessWithTransaction<T>(
            this Result self,
            EdoContext context,
            Func<Task<Result<T>>> f)
        {
            var (_, isFailure, error) = self;
            if (isFailure)
                return Task.FromResult(Result.Fail<T>(error));

            return WithTransactionScope(context, f);
        }


        public static async Task<Result<TK>> OnSuccessWithTransaction<T, TK>(
            this Task<Result<T>> self,
            EdoContext context,
            Func<T, Task<Result<TK>>> f)
        {
            var (_, isFailure, result, error) = await self.ConfigureAwait(Result.DefaultConfigureAwait);
            if (isFailure)
                return Result.Fail<TK>(error);

            return await WithTransactionScope(context, () => f(result));
        }


        public static async Task<Result> OnSuccessWithTransaction<T>(
            this Task<Result<T>> self,
            EdoContext context,
            Func<T, Task<Result>> f)
        {
            var (_, isFailure, result, error) = await self.ConfigureAwait(Result.DefaultConfigureAwait);
            if (isFailure)
                return Result.Fail(error);

            return await WithTransactionScope(context, () => f(result));
        }


        public static async Task<Result<T>> OnSuccessWithTransaction<T, TK>(
            this Result<TK> self,
            EdoContext context,
            Func<TK, Task<Result<T>>> f)
        {
            var (_, isFailure, result, error) = self;
            if (isFailure)
                return Result.Fail<T>(error);

            return await WithTransactionScope(context, () => f(result));
        }


        public static async Task<Result<T, TE>> OnSuccessWithTransaction<T, TE>(
            this Task<Result<T, TE>> self,
            EdoContext context,
            Func<T, Task<Result<T, TE>>> f)
        {
            var (_, isFailure, result, error) = await self.ConfigureAwait(Result.DefaultConfigureAwait);
            if (isFailure)
                return Result.Fail<T, TE>(error);

            return await WithTransactionScope(context, () => f(result));
        }


        public static async Task<Result> OnSuccessWithTransaction<T>(
            this Result<T> self,
            EdoContext context,
            Func<T, Task<Result>> f)
        {
            var (_, isFailure, result, error) = self;
            if (isFailure)
                return Result.Fail(error);

            return await WithTransactionScope(context, () => f(result));
        }


        private static Task<TResult> WithTransactionScope<TResult>(EdoContext context, Func<Task<TResult>> operation)
            where TResult : IResult
        {
            var strategy = context.Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync((object) null,
                async (dbContext, state, cancellationToken) =>
                {
                    // Nested transaction support. We can commit only on a top-level
                    var transaction = dbContext.Database.CurrentTransaction is null
                        ? await dbContext.Database.BeginTransactionAsync(cancellationToken)
                        : null;
                    try
                    {
                        var result = await operation().ConfigureAwait(Result.DefaultConfigureAwait);
                        if (result.IsSuccess)
                            transaction?.Commit();

                        return result;
                    }
                    finally
                    {
                        transaction?.Dispose();
                    }
                },
                // This delegate is not used in NpgSql.
                null);
        }
    }
}