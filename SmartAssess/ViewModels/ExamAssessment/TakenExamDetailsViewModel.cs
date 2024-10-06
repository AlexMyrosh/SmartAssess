using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace Presentation_Layer.ViewModels.ExamAssessment
{
    public class TakenExamDetailsViewModel
    {
        public string ExamName { get; set; }

        public string ExamDescription { get; set; }

        public bool IsExamAssessed { get; set; }

        public bool IsAssessedByAi { get; set; }

        public int TotalGrade { get; set; }

        public int MaxPossibleExamGrade { get; set; }

        public TimeSpan TakenTimeToComplete { get; set; }

        public int MinExamPassGrade { get; set; }

        public string? GeneralFeedback { get; set; }

        public List<QuestionAnswerViewModel> QuestionAnswers { get; set; }
    }
}