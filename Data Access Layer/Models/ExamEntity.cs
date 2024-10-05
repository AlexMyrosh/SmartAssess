using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Data_Access_Layer.Models.Enums;

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
        [DefaultValue(1)]
        public int MaxAttemptsAllowed { get; set; }

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
        public FinalGradeCalculationMethodEntity FinalGradeCalculationMethod { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        public virtual CourseEntity Course { get; set; }

        public virtual List<ExamQuestionEntity> Questions { get; set; }

        public virtual List<UserExamAttemptEntity> UserExamAttempts { get; set; }
    }
}
