using FluentValidation;
using Presentation_Layer.ViewModels.Exam;

namespace Presentation_Layer.FluentValidator.Exam
{
    public class ExamViewModelValidator : AbstractValidator<ExamViewModel>
    {
        public ExamViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.MaxAttemptsAllowed).NotEmpty().WithMessage("Max attempts number is required");
            RuleFor(x => x.ExamStartDateTime).NotEmpty().WithMessage("Start date is required");
            RuleFor(x => x.ExamEndDateTime).NotEmpty().WithMessage("End date is required");
            RuleFor(x => x.ExamDuration).NotEmpty().WithMessage("Exam duration value is required");
            RuleFor(x => x.FinalGradeCalculationMethod).NotEmpty().WithMessage("Final grade calculation method is required");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course Id is required");
            RuleFor(x => x.Questions).Must(x=>x.Count > 0).WithMessage("At least one question is required");
            RuleFor(x => x.MinimumPassGrade).LessThanOrEqualTo(x => x.Questions.Sum(q => q.MaxGrade)).WithMessage("Min pass grade should be less than max possible grade for all questions");
        }
    }
}
