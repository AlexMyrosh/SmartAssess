namespace Business_Logic_Layer.Models
{
    public class PaginationCourseModel
    {
        public string CurrentUserRole { get; set; }

        public List<CourseModel> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
