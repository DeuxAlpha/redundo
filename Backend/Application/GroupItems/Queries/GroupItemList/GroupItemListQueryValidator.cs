using FluentValidation;

namespace Application.GroupItems.Queries.GroupItemList
{
    public class GroupItemListQueryValidator : AbstractValidator<GroupItemListQuery>
    {
        public GroupItemListQueryValidator()
        {
            RuleFor(gi => gi.GroupId).NotEmpty();
        }
    }
}