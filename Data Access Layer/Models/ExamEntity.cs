using System.ComponentModel;
using Data_Access_Layer.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class ExamEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        
        [Required]
        [DefaultValue(SubjectEntity.NotSet)]
        public SubjectEntity Subject { get; set; }

        [Required]
        public DateTimeOffset ExamStartDateTime { get; set; }

        [Required]
        public DateTimeOffset ExamEndDateTime { get; set; }

        [Required]
        public TimeSpan ExamDuration { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsAssessedByAi { get; set; }

        [Required]
        public int MinimumPassGrade { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public virtual List<ExamQuestionEntity> Questions { get; set; }

        public virtual List<UserExamAttemptEntity> UserExamAttempts { get; set; }
    }
}
