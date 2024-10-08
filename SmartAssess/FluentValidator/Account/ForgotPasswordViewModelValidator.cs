using FluentValidation;
using Presentation_Layer.ViewModels.Account;

namespace Presentation_Layer.FluentValidator.Account
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please provide correct email").NotEmpty().WithMessage("Email is required");
        }
    }
}
