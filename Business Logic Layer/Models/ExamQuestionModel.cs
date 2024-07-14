namespace Business_Logic_Layer.Models
{
    public class ExamQuestionModel
    {
        public string QuestionText { get; set; }

        IEnumerable<QuestionOptionModel> Options { get; set; }

        public QuestionOptionModel CorrectAnswer { get; set; }

        public Guid ExamId { get; set; }

        public virtual ExamModel Exam { get; set; }
    }
}
