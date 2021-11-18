using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
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
                .NotNull()
                .WithState(_ => ErrorCodes.DbNameMissing);

            RuleFor(x => x.Graph)
                .NotEmpty()
                .NotNull()
                .WithState(_=>ErrorCodes.GraphNameMissing);

            RuleFor(x => x.Type)
                .NotEmpty()
                .NotNull();
        }
    }
}