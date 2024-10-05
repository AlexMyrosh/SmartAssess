using Presentation_Layer.ViewModels.Course.Shared;

namespace Presentation_Layer.ViewModels.Course
{
    public class CourseListWithPaginationViewModel
    {
        public List<CourseViewModel> Courses { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}