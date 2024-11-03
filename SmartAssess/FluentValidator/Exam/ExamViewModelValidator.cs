using FluentValidation;
using FluentValidator.Course;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Exam;

namespace FluentValidator.Exam
{
    public class ExamViewModelValidator : AbstractValidator<ExamViewModel>
    {
        public ExamViewModelValidator(IStringLocalizer<ExamViewModelValidator> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["NameRequired"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizer["DescriptionRequired"]);
            RuleFor(x => x.MaxAttemptsAllowed).NotEmpty().WithMessage(localizer["MaxAttemptsNumberRequired"]);
            RuleFor(x => x.ExamStartDateTime).NotEmpty().WithMessage(localizer["StartDateRequired"]);
            RuleFor(x => x.ExamEndDateTime).NotEmpty().WithMessage(localizer["EndDateRequired"]);
            RuleFor(x => x.ExamDuration).NotEmpty().WithMessage(localizer["ExamDurationValueRequired"]);
            RuleFor(x => x.FinalGradeCalculationMethod).NotEmpty().WithMessage(localizer["FinalGradeCalculationMethodRequired"]);
            RuleFor(x => x.CourseId).NotEmpty().WithMessage(localizer["CourseIdRequired"]);
            RuleFor(x => x.Questions).Must(x=>x.Count > 0).WithMessage(localizer["AtLeastOneQuestionRequired"]);
            RuleFor(x => x.MinimumPassGrade).LessThanOrEqualTo(x => x.Questions.Sum(q => q.MaxGrade)).WithMessage(localizer["MinPassGradeShouldBeLessThanTotalQuestionsGrade"]);
        }
    }
}
