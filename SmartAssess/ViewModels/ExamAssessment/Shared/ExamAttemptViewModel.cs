namespace Presentation_Layer.ViewModels.ExamAssessment.Shared
{
    public class ExamAttemptViewModel
    {
        public Guid AttemptId { get; set; }

        public bool IsExamAssessed { get; set; }

        public int TotalGrade { get; set; }
    }
}
