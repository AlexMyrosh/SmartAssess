using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace FluentValidator.ExamAssessment
{
    public class QuestionAnswerViewModelValidator : AbstractValidator<QuestionAnswerViewModel>
    {
        public QuestionAnswerViewModelValidator(IStringLocalizer<QuestionAnswerViewModelValidator> localizer)
        {
            RuleFor(x => x.QuestionAnswerId).NotEmpty().WithMessage(localizer["QuestionAnswerIdRequired"]);
        }
    }
}
