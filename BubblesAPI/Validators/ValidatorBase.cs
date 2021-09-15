using System.Linq;
using BubblesAPI.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Validators
{
    public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext,
            ValidationResult result)
        {
            var projection = result.Errors.Select(
                failure => new ValidationFailure(
                    failure.PropertyName,
                    ErrorCodes.BadRequest));
            return new ValidationResult(projection);
        }
    }
}