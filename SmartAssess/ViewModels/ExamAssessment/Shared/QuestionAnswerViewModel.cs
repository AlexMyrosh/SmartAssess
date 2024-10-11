namespace Presentation_Layer.ViewModels.ExamAssessment.Shared
{
    public class QuestionAnswerViewModel
    {
        public Guid QuestionAnswerId { get; set; }

        public string QuestionText { get; set; }

        public string? AnswerText { get; set; }

        public int QuestionMaxGrade { get; set; }

        public int Grade { get; set; }

        public string? QuestionAnswerFeedback { get; set; }
    }
}
