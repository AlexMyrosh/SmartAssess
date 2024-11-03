using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Account;

namespace FluentValidator.Account
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator(IStringLocalizer<ResetPasswordViewModelValidator> localizer)
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["PleaseProvideCorrectEmail"]).NotEmpty().WithMessage(localizer["EmailRequired"]);
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizer["NewPasswordRequired"]);
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(localizer["PasswordsDontMatch"]).NotEmpty().WithMessage(localizer["PleaseConfirmNewPassword"]);
        }
    }
}
