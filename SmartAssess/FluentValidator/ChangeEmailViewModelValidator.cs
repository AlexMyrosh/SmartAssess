using FluentValidation;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.FluentValidator
{
    public class ChangeEmailViewModelValidator : AbstractValidator<ChangeEmailViewModel>
    {
        public ChangeEmailViewModelValidator()
        {
            RuleFor(x => x.NewEmail).EmailAddress().WithMessage("Please provide correct email").NotEmpty().WithMessage("Email is required");
        }
    }
}
