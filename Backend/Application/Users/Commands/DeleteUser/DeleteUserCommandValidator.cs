using FluentValidation;

namespace Application.Users.Commands.DeleteUser
{
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
		public DeleteUserCommandValidator()
		{
			RuleFor(u => u.Id).NotEmpty();
			RuleFor(u => u.Password).NotEmpty();
		}
	}
}