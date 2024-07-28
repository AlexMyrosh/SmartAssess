namespace Presentation_Layer.ViewModels
{
    public class UserAnswerViewModel
    {
        public Guid? Id { get; set; }

        public string? AnswerText { get; set; }

        public int? Grade { get; set; }

        public string? Feedback { get; set; }

        public bool? IsDeleted { get; set; }

        public ExamQuestionViewModel? Question { get; set; }
    }
}
