using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class UserAnswerEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(1000)]
        public string? AnswerText { get; set; }

        public int? Grade { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        public Guid QuestionId { get; set; }
        public virtual ExamQuestionEntity Question { get; set; }

        [Required]
        public Guid UserExamAttemptId { get; set; }
        public virtual UserExamAttemptEntity UserExamAttempt { get; set; }
    }
}
