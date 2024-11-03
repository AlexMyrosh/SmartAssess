using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Account;

namespace FluentValidator.Account
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator(IStringLocalizer<ChangePasswordViewModelValidator> localizer)
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(localizer["CurrentPasswordIsRequired"]);
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizer["NewPasswordIsRequired"]);
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(localizer["PasswordsDontMatch"]).NotEmpty().WithMessage(localizer["PleaseConfirmNewPassword"]);
        }
    }
}
