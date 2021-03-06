using System;
using System.Linq;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace HappyTravel.Edo.Api.Infrastructure
{
    public class GenericValidator<T> : AbstractValidator<T>
    {
        public static Result Validate(Action<GenericValidator<T>> configureAction, T entity)
        {
            var validator = new GenericValidator<T>();
            configureAction(validator);
            return validator.GetValidationResult(entity);
        }


        private Result GetValidationResult(T entity)
        {
            var validationResult = base.Validate(entity);
            return validationResult.IsValid
                ? Result.Ok()
                : Result.Combine(validationResult
                    .Errors
                    .Select(e => Result.Fail(e.ErrorMessage))
                    .ToArray());
        }
    }
}