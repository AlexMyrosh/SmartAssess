using System.ComponentModel.DataAnnotations;

namespace Business_Logic_Layer.Models
{
    public class ExamModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public DateTime ExamStartDateTime { get; set; }

        public DateTime ExamEndDateTime { get; set; }

        public TimeSpan ExamDuration { get; set; }

        public bool IsAssessedByAi { get; set; }

        public bool IsDeleted { get; set; }

        public List<ExamQuestionModel> Questions { get; set; }

        public List<UserExamPassModel> StudentAnswers { get; set; }
    }
}
