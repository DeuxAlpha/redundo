using FluentValidation;

namespace Application.Items.Commands.CreateItem
{
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(i => i.Name).NotEmpty().MinimumLength(2);
        }
    }
}