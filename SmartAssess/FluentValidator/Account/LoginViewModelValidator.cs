using FluentValidation;
using Presentation_Layer.ViewModels.Account;

namespace Presentation_Layer.FluentValidator.Account
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
