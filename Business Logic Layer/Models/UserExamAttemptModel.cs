﻿namespace Business_Logic_Layer.Models
{
    public class UserExamAttemptModel
    {
        public Guid? Id { get; set; }

        public string? Feedback { get; set; }

        public bool IsExamAssessed { get; set; }

        public bool IsAssessedByAi { get; set; }

        public bool? IsDeleted { get; set; }

        public int? TotalGrade { get; set; }

        public UserModel? User { get; set; }

        public ExamModel? Exam { get; set; }

        public List<UserAnswerModel>? UserAnswers { get; set; }
    }
}
