namespace Presentation_Layer.ViewModels
{
    public class TeacherNoteViewModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int MinGrade { get; set; }

        public int MaxGrade { get; set; }

        public ExamQuestionViewModel Question { get; set; }
    }
}
