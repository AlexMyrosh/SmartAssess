using FluentValidation;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.FluentValidator
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage("Please confirm the password")
                .Equal(x=>x.NewPassword).WithMessage("Passwords do not match");
        }
    }
}
