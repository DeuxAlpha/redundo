using FluentValidation;

namespace Application.Users.Queries.UserDetail
{
    public class UserDetailQueryValidator : AbstractValidator<UserDetailQuery>
    {
        public UserDetailQueryValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
        }
    }
}