using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using FluentValidation;

namespace BubblesAPI.Validators
{
    public class LoginRequestValidator : ValidatorBase<LoginRequest>
    {
        public LoginRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithState(_ => ErrorCodes.EmailMissing);

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.PasswordMissing);
        }
    }
}