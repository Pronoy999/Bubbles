using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using FluentValidation;

namespace BubblesAPI.Validators
{
    public class CreateDbRequestValidator : ValidatorBase<CreateDbRequest>
    {
        public CreateDbRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.DbName)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.DbNameMissing);
        }
    }
}