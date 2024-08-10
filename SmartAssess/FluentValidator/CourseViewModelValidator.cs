using FluentValidation;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.FluentValidator
{
    public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
    {
        public CourseViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Course name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Course description is required");
        }
    }
}
