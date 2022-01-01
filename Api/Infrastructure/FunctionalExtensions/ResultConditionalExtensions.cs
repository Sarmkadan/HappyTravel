using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Edo.Api.Infrastructure.FunctionalExtensions
{
    public static class ResultConditionalExtensions
    {
        public static async Task<Result<T>> OnSuccessIf<T>(this Task<Result<T>> resultFunc, Func<T, bool> condition, Func<Task> action)
        {
            var result = await resultFunc;
            if (result.IsFailure)
                return result;

            if (condition(result.Value))
                await action();

            return result;
        }


        public static async Task<Result<T>> OnSuccessIf<T>(this Task<Result<T>> resultFunc, Func<T, bool> condition, Func<T, Task> action)
        {
            var result = await resultFunc;
            if (result.IsFailure)
                return result;

            if (condition(result.Value))
                await action(result.Value);

            return result;
        }


        public static async Task<Result<T>> OnSuccessIf<T>(this Result<T> resultFunc, Func<T, bool> condition, Func<Task> action)
        {
            var result = resultFunc;
            if (result.IsFailure)
                return result;

            if (condition(result.Value))
                await action();

            return result;
        }
    }
}