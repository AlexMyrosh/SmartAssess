namespace Business_Logic_Layer.Models
{
    public class ExamQuestionModel
    {
        public Guid Id { get; set; }

        public string QuestionText { get; set; }

        public ExamModel Exam { get; set; }

        public TeacherNoteModel TeacherNote { get; set; }

        public List<UserAnswerModel> StudentAnswers { get; set; }
    }
}
