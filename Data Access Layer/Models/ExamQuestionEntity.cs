using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class ExamQuestionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string QuestionText { get; set; }

        [Required]
        public int MaxGrade { get; set; }

        [MaxLength(500)]
        public string? TeacherNoteForAssessment { get; set; }

        [Required]
        public Guid ExamId { get; set; }
        public virtual ExamEntity Exam { get; set; }

        public virtual List<UserAnswerEntity> UserAnswers { get; set; }
    }
}
