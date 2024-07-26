using Data_Access_Layer.Models;

namespace Presentation_Layer.ViewModels
{
    public class UserExamAttemptViewModel
    {
        public Guid? Id { get; set; }

        public string? Feedback { get; set; }

        public bool? IsDeleted { get; set; }

        public int? TotalGrade { get; set; }

        public UserEntity? User { get; set; }

        public ExamViewModel? Exam { get; set; }

        public List<UserAnswerViewModel>? UserAnswers { get; set; }
    }
}
