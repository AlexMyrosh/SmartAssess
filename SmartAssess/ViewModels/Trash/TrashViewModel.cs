using Presentation_Layer.ViewModels.Trash.Shared;

namespace Presentation_Layer.ViewModels.Trash
{
    public class TrashViewModel
    {
        List<CourseViewModel> DeletedCourses { get; set; }

        List<ExamViewModel> DeletedExams { get; set; }
    }
}