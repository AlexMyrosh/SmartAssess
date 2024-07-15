namespace Business_Logic_Layer.Models
{
    public class TeacherNoteModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int MinGrade { get; set; }

        public int MaxGrade { get; set; }

        public ExamQuestionModel Question { get; set; }
    }
}
