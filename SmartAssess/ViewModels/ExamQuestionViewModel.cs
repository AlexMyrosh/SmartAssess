namespace Presentation_Layer.ViewModels
{
    public class ExamQuestionViewModel
    {
        public string QuestionText { get; set; }

        IEnumerable<QuestionOptionViewModel> Options { get; set; }

        public QuestionOptionViewModel CorrectAnswer { get; set; }
    }
}
