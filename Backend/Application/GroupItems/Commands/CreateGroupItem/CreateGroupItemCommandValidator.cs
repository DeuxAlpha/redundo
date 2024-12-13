using FluentValidation;

namespace Application.GroupItems.Commands.CreateGroupItem
{
    public class CreateGroupItemCommandValidator : AbstractValidator<CreateGroupItemCommand>
    {
        public CreateGroupItemCommandValidator()
        {
            RuleFor(gi => gi.ItemId).NotEmpty();
            RuleFor(gi => gi.GroupId).NotEmpty();
            RuleFor(gi => gi.ItemStatusId).NotEmpty();
            RuleFor(gi => gi.Notes).MaximumLength(255);
        }
    }
}