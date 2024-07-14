namespace Business_Logic_Layer.Models
{
    public class QuestionOptionModel
    {
        public string OptionText { get; set; }

        public Guid ExamQuestionId { get; set; }

        public virtual ExamQuestionModel ExamQuestion { get; set; }
    }
}
