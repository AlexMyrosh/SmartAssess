using Presentation_Layer.ViewModels.Course.Shared;

namespace Presentation_Layer.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LongDescription { get; set; }

        public bool IsCurrentUserApplied { get; set; }

        public List<UserViewModel> AppliedUsers { get; set; }

        public List<string> TeacherIds { get; set; }

        public List<ExamViewModel> Exams { get; set; }
    }
}
