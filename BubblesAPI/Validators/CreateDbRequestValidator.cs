using BubblesAPI.DTOs;
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
                .NotNull();
        }
    }
}