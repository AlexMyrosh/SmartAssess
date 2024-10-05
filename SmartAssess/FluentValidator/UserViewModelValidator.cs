using FluentValidation;
using Presentation_Layer.ViewModels.Shared;

namespace Presentation_Layer.FluentValidator
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        }
    }
}
