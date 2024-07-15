using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class ExamQuestionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public Guid ExamId { get; set; }

        public virtual ExamEntity Exam { get; set; }
    }
}
