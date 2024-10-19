using FluentValidation;
using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace Presentation_Layer.FluentValidator.ExamAssessment
{
    public class QuestionAnswerViewModelValidator : AbstractValidator<QuestionAnswerViewModel>
    {
        public QuestionAnswerViewModelValidator()
        {
            RuleFor(x => x.QuestionAnswerId).NotEmpty().WithMessage("Question answer Id is required");
        }
    }
}
