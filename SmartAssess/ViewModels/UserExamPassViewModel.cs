using Data_Access_Layer.Models;

namespace Presentation_Layer.ViewModels
{
    public class UserExamPassViewModel
    {
        public Guid? Id { get; set; }

        public string? ExamName { get; set; }

        public string? ExamDescription { get; set; }

        public string? ExamSubject { get; set; }

        public string? Feedback { get; set; }

        public int? TotalGrade { get; set; }

        public Guid? ExamId { get; set; }

        public UserEntity? User { get; set; }

        public List<UserAnswerViewModel> UserAnswers { get; set; }
    }
}
