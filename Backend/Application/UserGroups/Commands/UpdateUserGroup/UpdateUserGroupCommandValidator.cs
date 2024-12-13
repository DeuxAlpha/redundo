using FluentValidation;

namespace Application.UserGroups.Commands.UpdateUserGroup
{
    public class UpdateUserGroupCommandValidator : AbstractValidator<UpdateUserGroupCommand>
    {
        public UpdateUserGroupCommandValidator()
        {
            RuleFor(ug => ug.UserId).NotEmpty();
            RuleFor(ug => ug.GroupId).NotEmpty();
        }
    }
}