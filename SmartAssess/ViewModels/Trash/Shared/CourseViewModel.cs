namespace Presentation_Layer.ViewModels.Trash.Shared
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DeletedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; }
    }
}