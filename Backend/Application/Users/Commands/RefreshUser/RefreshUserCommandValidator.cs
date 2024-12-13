using FluentValidation;

namespace Application.Users.Commands.RefreshUser
{
    public class RefreshUserCommandValidator : AbstractValidator<RefreshUserCommand>
    {
        public RefreshUserCommandValidator()
        {
            RuleFor(u => u.JwtToken).NotEmpty();
            RuleFor(u => u.RefreshToken).NotEmpty();
        }
    }
}