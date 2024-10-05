using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.ViewModels.Exam
{
    public class ExamResultViewModel
    {
        public string ExamName { get; set; }

        public List<UserExamAttemptViewModel> UserAttempts { get; set; }

        public int MaxPossibleExamGrade { get; set; }

        public int MinimumPassGrade { get; set; }
    }
}