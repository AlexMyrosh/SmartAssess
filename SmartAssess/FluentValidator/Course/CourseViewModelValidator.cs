using FluentValidation;
using Presentation_Layer.ViewModels.Course.Shared;

namespace Presentation_Layer.FluentValidator.Course
{
    public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
    {
        public CourseViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.LongDescription).NotEmpty().WithMessage("Description is required");
        }
    }
}