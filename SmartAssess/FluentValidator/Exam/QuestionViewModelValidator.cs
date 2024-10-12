using FluentValidation;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.FluentValidator.Exam
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(x => x.MaxGrade).NotEmpty().WithMessage("Max grade is required");
            RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question text is required");
        }
    }
}
