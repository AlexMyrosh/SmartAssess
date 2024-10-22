using FluentValidation;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace Presentation_Layer.FluentValidator.ExamAssessment
{
    public class TakeExamViewModelValidator : AbstractValidator<TakeExamViewModel>
    {
        public TakeExamViewModelValidator()
        {
            RuleFor(x => x.AttemptId).NotEmpty().WithMessage("Attempt Id is null");
            //RuleFor(x => x.TakenTimeToComplete).NotEmpty().WithMessage("Taken Time To Complete is null");
        }
    }
}
