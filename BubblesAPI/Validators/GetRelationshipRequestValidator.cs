using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using FluentValidation;

namespace BubblesAPI.Validators
{
    public class GetRelationshipRequestValidator : ValidatorBase<GetRelationshipRequest>
    {
        public GetRelationshipRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.DbName)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.DbNameMissing);

            RuleFor(x => x.RelationshipId)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.RelationshipIdMissing);
        }
    }
}