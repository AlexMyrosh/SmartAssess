using Presentation_Layer.ViewModels.Enums;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.ViewModels.Exam
{
    public class ExamResultViewModel
    {
        public string ExamName { get; set; }

        public Dictionary<UserViewModel, ExamAttemptsViewModel> UserAttempts { get; set; }

        public int MaxPossibleExamGrade { get; set; }

        public int MinimumPassGrade { get; set; }
    }
}