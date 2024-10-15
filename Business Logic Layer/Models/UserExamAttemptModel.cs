using Business_Logic_Layer.Models.Enums;

namespace Business_Logic_Layer.Models
{
    public class UserExamAttemptModel
    {
        public Guid? Id { get; set; }

        public string? Feedback { get; set; }

        public bool IsExamAssessed { get; set; }

        public bool IsAssessedByAi { get; set; }

        public TimeSpan TakenTimeToComplete { get; set; }

        public int? TotalGrade { get; set; }

        public DateTimeOffset AttemptStarterAt { get; set; }

        public ExamAttemptStatusModel Status { get; set; }

        public UserModel? User { get; set; }

        public ExamModel? Exam { get; set; }

        public List<UserAnswerModel>? UserAnswers { get; set; }
    }
}
