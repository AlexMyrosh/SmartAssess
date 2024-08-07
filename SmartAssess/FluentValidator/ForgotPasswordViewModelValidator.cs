﻿using FluentValidation;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.FluentValidator
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
