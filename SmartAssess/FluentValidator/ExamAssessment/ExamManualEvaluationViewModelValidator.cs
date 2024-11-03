using FluentValidation;
using FluentValidator.Exam;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace FluentValidator.ExamAssessment
{
    public class ExamManualEvaluationViewModelValidator : AbstractValidator<ExamManualEvaluationViewModel>
    {
        public ExamManualEvaluationViewModelValidator(IStringLocalizer<ExamManualEvaluationViewModelValidator> localizer)
        {
            RuleFor(x => x.ExamId).NotEmpty().WithMessage(localizer["ExamIdNull"]);
            RuleFor(x => x.ExamAttemptId).NotEmpty().WithMessage(localizer["ExamAttemptIdNull"]);
        }
    }
}
