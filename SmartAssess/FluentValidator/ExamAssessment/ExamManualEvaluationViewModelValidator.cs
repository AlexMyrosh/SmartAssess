using FluentValidation;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace Presentation_Layer.FluentValidator.ExamAssessment
{
    public class ExamManualEvaluationViewModelValidator : AbstractValidator<ExamManualEvaluationViewModel>
    {
        public ExamManualEvaluationViewModelValidator()
        {
            RuleFor(x => x.ExamId).NotEmpty().WithMessage("Exam id is null");
            RuleFor(x => x.ExamAttemptId).NotEmpty().WithMessage("Exam attempt id is null");
        }
    }
}
