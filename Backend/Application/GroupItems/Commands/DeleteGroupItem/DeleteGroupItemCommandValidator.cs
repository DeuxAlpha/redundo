using FluentValidation;

namespace Application.GroupItems.Commands.DeleteGroupItem
{
    public class DeleteGroupItemCommandValidator : AbstractValidator<DeleteGroupItemCommand>
    {
        public DeleteGroupItemCommandValidator()
        {
            RuleFor(gi => gi.GroupId).NotEmpty();
            RuleFor(gi => gi.ItemId).NotEmpty();
        }
    }
}