using FluentValidation;
using Presentation_Layer.ViewModels.Account;

namespace Presentation_Layer.FluentValidator.Account
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please provide correct email").NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage("Passwords don't match").NotEmpty().WithMessage("Please confirm new password");
        }
    }
}
