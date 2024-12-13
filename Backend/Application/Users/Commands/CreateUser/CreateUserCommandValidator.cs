using FluentValidation;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Username).NotEmpty().MinimumLength(6);
            RuleFor(u => u.Password).NotEmpty().MinimumLength(7);
        }
    }
}