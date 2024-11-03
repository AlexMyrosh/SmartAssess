using ViewModels.Enums;

namespace Presentation_Layer.ViewModels.ExamAssessment.Shared
{
    public class ExamAttemptViewModel
    {
        public Guid AttemptId { get; set; }

        public bool IsExamAssessed { get; set; }

        public bool IsExamAssessedByAi { get; set; }

        public ExamAttemptStatusViewModel Status { get; set; }

        public int TotalGrade { get; set; }

        public TimeSpan TakenTimeToComplete { get; set; }
    }
}
