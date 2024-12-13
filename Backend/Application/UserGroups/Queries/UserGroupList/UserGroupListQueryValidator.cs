using FluentValidation;

namespace Application.UserGroups.Queries.UserGroupList
{
    public class UserGroupListQueryValidator : AbstractValidator<UserGroupListQuery>
    {
        public UserGroupListQueryValidator()
        {
            RuleFor(ug => ug.GroupId).NotEmpty();
        }
    }
}