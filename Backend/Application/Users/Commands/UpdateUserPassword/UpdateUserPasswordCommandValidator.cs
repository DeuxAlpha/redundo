using FluentValidation;

namespace Application.Users.Commands.UpdateUserPassword
{
	public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
	{
		public UpdateUserPasswordCommandValidator()
		{
			RuleFor(u => u.Id).NotEmpty();
			RuleFor(u => u.OldPassword).NotEmpty();
			RuleFor(u => u.NewPassword).NotEmpty().MinimumLength(7);
		}		
	}
}