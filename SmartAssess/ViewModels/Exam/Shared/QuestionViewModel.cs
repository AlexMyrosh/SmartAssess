namespace Presentation_Layer.ViewModels.Exam.Shared
{
    public class QuestionViewModel
    {
        public Guid? QuestionId { get; set; }

        public string QuestionText { get; set; }

        public int MaxGrade { get; set; }
    }
}
