namespace Presentation_Layer.ViewModels
{
    public class ExamQuestionViewModel
    {
        public Guid? Id { get; set; }

        public string QuestionText { get; set; }

        public string? UserAnswer { get; set; }

        public ExamViewModel? Exam { get; set; }

        public TeacherNoteViewModel? TeacherNote { get; set; }

        public List<UserAnswerViewModel>? StudentAnswers { get; set; }
    }
}