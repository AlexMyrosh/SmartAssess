using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Account;

namespace FluentValidator.Account
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator(IStringLocalizer<RegisterViewModelValidator> localizer)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizer["FirstNameRequired"]);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizer["LastNameRequired"]);
            RuleFor(x => x.UserName).NotEmpty().WithMessage(localizer["UsernameRequired"]);
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["PleaseProvideCorrectEmail"]).NotEmpty().WithMessage(localizer["EmailRequired"]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizer["PasswordRequired"]);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(localizer["PasswordsDontMatch"]).NotEmpty().WithMessage(localizer["PleaseConfirmPassword"]);
        }
    }
}
