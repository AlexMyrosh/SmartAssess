using ViewModels.Enums;

namespace Presentation_Layer.ViewModels.Exam.Shared
{
    public class UserExamAttemptViewModel
    {
        public Guid Id { get; set; }

        public bool IsAssessed { get; set; }

        public bool IsAssessedByAi { get; set; }

        public double? TotalGrade { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public DateTimeOffset AttemptStarterAt { get; set; }

        public TimeSpan TakenTimeToComplete { get; set; }

        public ExamAttemptStatusViewModel Status { get; set; }
    }
}
