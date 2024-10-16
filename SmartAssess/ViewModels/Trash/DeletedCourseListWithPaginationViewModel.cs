using Presentation_Layer.ViewModels.Trash.Shared;

namespace Presentation_Layer.ViewModels.Trash
{
    public class DeletedCourseListWithPaginationViewModel
    {
        public string SearchQuery { get; set; }

        public List<CourseViewModel> Courses { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}