using FluentValidation;
using Presentation_Layer.ViewModels.Exam;

namespace Presentation_Layer.FluentValidator.Exam
{
    public class ExamViewModelValidator : AbstractValidator<ExamViewModel>
    {
        public ExamViewModelValidator()
        {
            RuleFor(x => x.MinimumPassGrade).LessThanOrEqualTo(x => x.Questions.Sum(q => q.MaxGrade)).WithMessage("Min pass grade should be less than max possible grade");
        }
    }
}
