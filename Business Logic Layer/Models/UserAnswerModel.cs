namespace Business_Logic_Layer.Models
{
    public class UserAnswerModel
    {
        public Guid? Id { get; set; }

        public string? AnswerText { get; set; }

        public int? Grade { get; set; }

        public string? Feedback { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? QuestionId { get; set; }

        public ExamQuestionModel? Question { get; set; }

        public UserExamAttemptModel? UserExamAttempt { get; set; }
    }
}
