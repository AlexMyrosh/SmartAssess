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

        public Guid? TeacherNoteId { get; set; }

        public virtual TeacherNoteEntity TeacherNote { get; set; }

        public List<UserAnswerEntity> StudentAnswers { get; set; }
    }
}
