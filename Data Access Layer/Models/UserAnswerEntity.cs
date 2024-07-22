using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class UserAnswerEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string? Answer { get; set; }

        public int? Grade { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        public virtual ExamQuestionEntity Question { get; set; }

        [Required]
        public Guid StudentExamPassId { get; set; }

        public virtual UserExamPassEntity StudentExamPass { get; set; }
    }
}
