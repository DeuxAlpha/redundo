using FluentValidation;

namespace Application.UserGroups.Commands.CreateUserGroup
{
    public class CreateUserGroupCommandValidator : AbstractValidator<CreateUserGroupCommand>
    {
        public CreateUserGroupCommandValidator()
        {
            RuleFor(ug => ug.UserId).NotEmpty();
            RuleFor(ug => ug.GroupId).NotEmpty();
        }
    }
}