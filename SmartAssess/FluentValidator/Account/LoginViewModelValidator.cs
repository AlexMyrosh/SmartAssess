using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Account;

namespace FluentValidator.Account
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator(IStringLocalizer<LoginViewModelValidator> localizer)
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(localizer["UsernameIsRequired"]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizer["PasswordIsRequired"]);
        }
    }
}
