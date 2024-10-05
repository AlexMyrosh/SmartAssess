using Presentation_Layer.ViewModels.Course.Shared;
using Presentation_Layer.ViewModels.Enums;

namespace Presentation_Layer.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LongDescription { get; set; }

        public List<UserViewModel> AppliedUsers { get; set; }

        public List<Guid> TeacherIds { get; set; }

        public List<ExamViewModel> Exams { get; set; }
    }
}
