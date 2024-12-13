using FluentValidation;

namespace Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupCommandValidator()
        {
            RuleFor(g => g.Id).NotEmpty();
            RuleFor(g => g.OwnerId).NotEmpty();
        }
    }
}