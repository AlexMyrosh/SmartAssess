namespace Presentation_Layer.ViewModels.ExamAssessment.Shared
{
    public class TakenExamCoursesViewModel
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; }

        public List<TakenExamsViewModel> TakenExams { get; set; }
    }
}
