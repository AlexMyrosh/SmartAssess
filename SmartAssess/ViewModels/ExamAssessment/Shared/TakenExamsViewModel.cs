using ViewModels.Enums;

namespace Presentation_Layer.ViewModels.ExamAssessment.Shared
{
    public class TakenExamsViewModel
    {
        public Guid ExamId { get; set; }

        public string ExamName { get; set; }

        public int ExamMaxPossibleExamGrade { get; set; }

        public int ExamMinPassGrade { get; set; }

        public FinalGradeCalculationMethodViewModel FinalGradeCalculationMethod { get; set; }

        public List<ExamAttemptViewModel> ExamAttempts { get; set; }

        public double? FinalAttemptGrade { get; set; }
    }
}
