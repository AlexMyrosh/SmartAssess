using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace FluentValidator.ExamAssessment
{
    public class TakeExamViewModelValidator : AbstractValidator<TakeExamViewModel>
    {
        public TakeExamViewModelValidator(IStringLocalizer<TakeExamViewModelValidator> localizer)
        {
            RuleFor(x => x.AttemptId).NotEmpty().WithMessage(localizer["AttemptIdNull"]);
        }
    }
}
