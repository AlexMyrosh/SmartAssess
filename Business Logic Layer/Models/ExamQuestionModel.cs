namespace Business_Logic_Layer.Models
{
    public class ExamQuestionModel
    {
        public Guid? Id { get; set; }

        public string? QuestionText { get; set; }

        public int? MaxGrade { get; set; }

        public bool? IsDeleted { get; set; }

        public ExamModel? Exam { get; set; }

        public List<UserAnswerModel>? UserAnswers { get; set; }
    }
}