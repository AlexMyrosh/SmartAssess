namespace Presentation_Layer.ViewModels
{
    public class ExamQuestionViewModel
    {
        public Guid? Id { get; set; }

        public string? QuestionText { get; set; }

        public int? MaxGrade { get; set; }

        public string? TeacherNoteForAssessment { get; set; }

        public bool? IsDeleted { get; set; }

        public ExamViewModel? Exam { get; set; }

        public List<UserAnswerViewModel>? UserAnswers { get; set; }
    }
}