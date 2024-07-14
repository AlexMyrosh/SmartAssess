namespace Data_Access_Layer.Models
{
    public class QuestionOptionEntity
    {
        public Guid Id { get; set; }

        public string OptionText { get; set; }

        public Guid ExamQuestionId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ExamQuestionEntity ExamQuestion { get; set; }
    }
}
