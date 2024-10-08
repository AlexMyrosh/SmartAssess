using FluentValidation;
using Presentation_Layer.ViewModels.Account;

namespace Presentation_Layer.FluentValidator.Account
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage("Passwords don't match").NotEmpty().WithMessage("Please confirm new password");
        }
    }
}
