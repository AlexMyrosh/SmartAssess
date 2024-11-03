using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Account;

namespace FluentValidator.Account
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator(IStringLocalizer<ForgotPasswordViewModelValidator> localizer)
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["PleaseProvideCorrectEmail"]).NotEmpty().WithMessage(localizer["EmailIsRequired"]);
        }
    }
}
