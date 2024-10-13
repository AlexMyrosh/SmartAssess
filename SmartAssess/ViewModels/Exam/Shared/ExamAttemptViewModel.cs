using Presentation_Layer.ViewModels.Enums;

namespace Presentation_Layer.ViewModels.Exam.Shared
{
    public class ExamAttemptViewModel
    {
        public Guid AttemptId { get; set; }

        public bool IsExamAssessed { get; set; }

        public bool IsExamAssessedByAi { get; set; }

        public double TotalGrade { get; set; }

        public TimeSpan TakenTimeToComplete { get; set; }

        public ExamAttemptStatusViewModel Status { get; set; }

        public DateTimeOffset AttemptStarterAt { get; set; }
    }
}
