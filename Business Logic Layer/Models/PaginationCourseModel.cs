namespace Business_Logic_Layer.Models
{
    public class PaginationCourseModel
    {
        public IEnumerable<CourseModel> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
