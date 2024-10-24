﻿using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace Presentation_Layer.ViewModels.ExamAssessment
{
    public class ExamManualEvaluationViewModel
    {
        public Guid ExamAttemptId { get; set; }

        public Guid ExamId { get; set; }

        public string ExamName { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string? GeneralFeedback { get; set; }

        public int ExamMinimumPassGrade { get; set; }

        public List<QuestionAnswerViewModel> UserAnswers { get; set; }
    }
}