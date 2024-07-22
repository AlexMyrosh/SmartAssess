namespace Business_Logic_Layer.Models
{
    public class UserAnswerModel
    {
        public Guid Id { get; set; }

        public string Answer { get; set; }

        public int Grade { get; set; }

        public Guid QuestionId { get; set; }

        public ExamQuestionModel Question { get; set; }

        public UserExamPassModel StudentExamPass { get; set; }
    }
}
