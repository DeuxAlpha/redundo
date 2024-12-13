using FluentValidation;

namespace Application.GroupItems.Commands.UpdateGroupItem
{
    public class UpdateGroupItemCommandValidator : AbstractValidator<UpdateGroupItemCommand>
    {
        public UpdateGroupItemCommandValidator()
        {
            RuleFor(gi => gi.GroupId).NotEmpty();
            RuleFor(gi => gi.ItemId).NotEmpty();
            RuleFor(gi => gi.ItemStatusId).NotEmpty();
        }
    }
}