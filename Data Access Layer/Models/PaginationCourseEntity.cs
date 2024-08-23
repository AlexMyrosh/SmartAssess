namespace Data_Access_Layer.Models
{
    public class PaginationCourseEntity
    {
        public IEnumerable<CourseEntity> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
