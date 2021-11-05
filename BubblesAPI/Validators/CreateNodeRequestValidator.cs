using BubblesAPI.DTOs;
using FluentValidation;

namespace BubblesAPI.Validators
{
    public class CreateNodeRequestValidator : ValidatorBase<CreateNodeRequest>
    {
        public CreateNodeRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Database)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Graph)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Type)
                .NotEmpty()
                .NotNull();
        }
    }
}