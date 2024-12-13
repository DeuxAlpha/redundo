using FluentValidation;

namespace Application.Groups.Queries.GroupDetail
{
    public class GroupDetailQueryValidator : AbstractValidator<GroupDetailQuery>
    {
        public GroupDetailQueryValidator()
        {
            RuleFor(g => g.Id).NotEmpty();
        }
    }
}