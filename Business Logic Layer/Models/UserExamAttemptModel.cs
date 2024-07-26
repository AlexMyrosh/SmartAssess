using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Models
{
    public class UserExamAttemptModel
    {
        public Guid Id { get; set; }

        public string Feedback { get; set; }

        public bool IsDeleted { get; set; }

        public int? TotalGrade { get; set; }

        public UserEntity? User { get; set; }

        public ExamModel Exam { get; set; }

        public List<UserAnswerModel> UserAnswers { get; set; }
    }
}
