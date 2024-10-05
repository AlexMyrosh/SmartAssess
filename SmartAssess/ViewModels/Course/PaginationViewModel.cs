namespace Presentation_Layer.ViewModels.Course
{
    public class PaginationViewModel
    {
        public int TotalCourses { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCourses / (double)PageSize);
    }
}
