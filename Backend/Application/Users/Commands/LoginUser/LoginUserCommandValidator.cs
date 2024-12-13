using FluentValidation;

namespace Application.Users.Commands.LoginUser
{
	public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
		public LoginUserCommandValidator()
		{
			RuleFor(u => u.Username).NotEmpty();
			RuleFor(u => u.Password).NotEmpty();
		}
	}
}