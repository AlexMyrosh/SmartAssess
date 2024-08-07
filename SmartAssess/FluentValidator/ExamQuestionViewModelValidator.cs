﻿using FluentValidation;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.FluentValidator
{
    public class ExamQuestionViewModelValidator : AbstractValidator<ExamQuestionViewModel>
    {
        public ExamQuestionViewModelValidator()
        {
            RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question text is required");
            RuleFor(x => x.MaxGrade).NotEmpty().WithMessage("Max Grade is required");
        }
    }
}
