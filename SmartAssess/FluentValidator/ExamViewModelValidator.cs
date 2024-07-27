using FluentValidation;
using Presentation_Layer.Enums;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.FluentValidator
{
    public class ExamViewModelValidator : AbstractValidator<ExamViewModel>
    {
        public ExamViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Subject).NotEqual(SubjectViewModel.NotSet).WithMessage("Subject should be set");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required");
            RuleFor(x => x.ExamStartDateTime).NotEmpty().WithMessage("Exam start time is required");
            RuleFor(x => x.ExamEndDateTime).NotEmpty().WithMessage("Exam end time is required");
            RuleFor(x => x.MinimumPassGrade).NotNull().WithMessage("Minimal grade pass is required");
            RuleFor(x => x.MinimumPassGrade).GreaterThanOrEqualTo(0).WithMessage("Minimal grade should be more or equal than 0");
            RuleFor(x => x.ExamDuration).NotEmpty().WithMessage("Exam Duration is required");
        }
    }
}
