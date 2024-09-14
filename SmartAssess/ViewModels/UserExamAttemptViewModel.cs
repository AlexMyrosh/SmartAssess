﻿namespace Presentation_Layer.ViewModels
{
    public class UserExamAttemptViewModel
    {
        public Guid? Id { get; set; }

        public string? Feedback { get; set; }

        public bool IsExamAssessed { get; set; }

        public bool IsAssessedByAi { get; set; }

        public bool? IsDeleted { get; set; }

        public int? TotalGrade { get; set; }

        public UserViewModel? User { get; set; }

        public ExamViewModel? Exam { get; set; }

        public List<UserAnswerViewModel>? UserAnswers { get; set; }
    }
}
