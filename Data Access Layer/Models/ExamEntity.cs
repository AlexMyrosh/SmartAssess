using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class ExamEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public DateTime ExamStartDateTime { get; set; }

        [Required]
        public DateTime ExamEndDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public List<ExamQuestionEntity> Questions { get; set; }

        public List<UserExamPassEntity> UserExamPasses { get; set; }
    }
}
