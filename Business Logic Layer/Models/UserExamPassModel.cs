using Data_Access_Layer.Models;

namespace Business_Logic_Layer.Models
{
    public class UserExamPassModel
    {
        public Guid Id { get; set; }

        public string Feedback { get; set; }

        public int? TotalGrade { get; set; }

        public List<UserAnswerModel> UserAnswers { get; set; }

        public Guid? ExamId { get; set; }

        public ExamModel Exam { get; set; }

        public UserEntity? User { get; set; }
    }
}
