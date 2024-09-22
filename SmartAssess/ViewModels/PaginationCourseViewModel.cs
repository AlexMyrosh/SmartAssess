namespace Presentation_Layer.ViewModels
{
    public class PaginationCourseViewModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<CourseViewModel> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public CourseViewModel CreatedCourse { get; set; }
    }

}
