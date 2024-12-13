using FluentValidation;

namespace Application.UserGroups.Commands.DeleteUserGroup
{
    public class DeleteUserGroupCommandValidator : AbstractValidator<DeleteUserGroupCommand>
    {
        public DeleteUserGroupCommandValidator()
        {
            RuleFor(ug => ug.UserId).NotEmpty();
            RuleFor(ug => ug.GroupId).NotEmpty();
        }
    }
}