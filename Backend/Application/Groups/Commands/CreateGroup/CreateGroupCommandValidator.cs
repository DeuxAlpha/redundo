using FluentValidation;

namespace Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator()
        {
            RuleFor(g => g.Name).MinimumLength(3).NotEmpty();
            RuleFor(g => g.GroupCreatorId).NotEmpty();
        }
    }
}