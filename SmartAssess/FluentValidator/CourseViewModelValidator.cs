using FluentValidation;
using Presentation_Layer.ViewModels.Old;

namespace Presentation_Layer.FluentValidator
{
    public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
    {
        public CourseViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Course name is required");
            RuleFor(x => x.LongDescription).NotEmpty().WithMessage("Course long description is required");
        }
    }
}
