namespace Presentation_Layer.ViewModels.Course
{
    public class AllCoursesViewModel
    {
        public string SearchQuery { get; set; }

        public CourseListWithPaginationViewModel CourseListWithPagination { get; set; }
    }
}