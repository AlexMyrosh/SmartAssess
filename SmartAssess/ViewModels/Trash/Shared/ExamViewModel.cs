namespace Presentation_Layer.ViewModels.Trash.Shared
{
    public class ExamViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? ExamStartDateTime { get; set; }

        public DateTimeOffset? ExamEndDateTime { get; set; }

        public string CourseName { get; set; }

        public string DeletedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; }
    }
}
