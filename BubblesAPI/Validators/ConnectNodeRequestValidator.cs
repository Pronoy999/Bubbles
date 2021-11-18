using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using FluentValidation;

namespace BubblesAPI.Validators
{
    public class ConnectNodeRequestValidator : ValidatorBase<ConnectNodeRequest>
    {
        public ConnectNodeRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.DbName)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.DbNameMissing);

            RuleFor(x => x.LeftNodeId)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.NodeIdMissing);

            RuleFor(x => x.RightNodeId)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.NodeIdMissing);

            RuleFor(x => x.RelationshipType)
                .NotEmpty()
                .NotNull()
                .WithState(_ => ErrorCodes.RelationshipTypeMissing);
        }
    }
}