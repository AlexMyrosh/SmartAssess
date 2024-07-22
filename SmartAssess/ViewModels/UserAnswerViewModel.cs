namespace Presentation_Layer.ViewModels
{
    public class UserAnswerViewModel
    {
        public Guid Id { get; set; }

        public string? QuestionText { get; set; }

        public string? AnswerText { get; set; }

        public int? Grade { get; set; }

        public Guid QuestionId { get; set; }
    }
}
