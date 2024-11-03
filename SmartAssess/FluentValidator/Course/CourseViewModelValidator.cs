using FluentValidation;
using FluentValidator.Account;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Course.Shared;

namespace FluentValidator.Course
{
    public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
    {
        public CourseViewModelValidator(IStringLocalizer<CourseViewModelValidator> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["NameRequired"]);
            RuleFor(x => x.LongDescription).NotEmpty().WithMessage(localizer["DescriptionRequired"]);
        }
    }
}