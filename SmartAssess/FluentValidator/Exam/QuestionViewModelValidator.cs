﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace FluentValidator.Exam
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator(IStringLocalizer<QuestionViewModelValidator> localizer)
        {
            RuleFor(x => x.MaxGrade).NotEmpty().WithMessage(localizer["MaxGradeRequired"]);
            RuleFor(x => x.MaxGrade).GreaterThan(0).WithMessage(localizer["MaxGradeRequired"]);
            RuleFor(x => x.QuestionText).NotEmpty().WithMessage(localizer["QuestionTextRequired"]);
            RuleFor(x => x.QuestionText).NotNull().WithMessage(localizer["QuestionTextRequired"]);
        }
    }
}
