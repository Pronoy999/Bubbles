using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using FluentValidation;

namespace BubblesAPI.Validators
{
    public class RegisterUserRequestValidator : ValidatorBase<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithState(_ => ErrorCodes.FirstNameMissing);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithState(_ => ErrorCodes.LastNameMissing);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithState(_ => ErrorCodes.EmailMissing);

            RuleFor(x => x.Password)
                .NotEmpty().WithState(_ => ErrorCodes.PasswordMissing);
        }
    }
}