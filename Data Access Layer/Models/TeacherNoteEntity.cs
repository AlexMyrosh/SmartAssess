namespace Data_Access_Layer.Models
{
    public class TeacherNoteEntity
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int MinGrade { get; set; }

        public int MaxGrade { get; set; }

        public ExamQuestionEntity Question { get; set; }

        public Guid QuestionId { get; set; }
    }
}