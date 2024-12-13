using FluentValidation;

namespace Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidator()
        {
            RuleFor(g => g.Id).NotEmpty();
        }
    }
}