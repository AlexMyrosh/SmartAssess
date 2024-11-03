using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Account;

namespace FluentValidator.Account
{
    public class ChangeEmailViewModelValidator : AbstractValidator<ChangeEmailViewModel>
    {
        public ChangeEmailViewModelValidator(IStringLocalizer<ChangeEmailViewModelValidator> localizer)
        {
            RuleFor(x => x.NewEmail).EmailAddress().WithMessage(localizer["PleaseProvideCorrectEmail"]).NotEmpty().WithMessage(localizer["EmailIsRequired"]);
        }
    }
}
