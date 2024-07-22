using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class UserExamPassEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string? Feedback { get; set; }

        public string? UserId { get; set; }

        public virtual UserEntity User { get; set; }

        [Required]
        public Guid ExamId { get; set; }

        public virtual ExamEntity Exam { get; set; }

        public virtual List<UserAnswerEntity> UserAnswers { get; set; }
    }
}