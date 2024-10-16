using Presentation_Layer.ViewModels.Trash.Shared;

namespace Presentation_Layer.ViewModels.Trash
{
    public class TrashViewModel
    {
        public DeletedCourseListWithPaginationViewModel DeletedCoursesWithPagination { get; set; }

        public List<ExamViewModel> DeletedExams { get; set; }

        public List<UserViewModel> DeletedUsers { get; set; }
    }
}