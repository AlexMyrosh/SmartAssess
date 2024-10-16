namespace Data_Access_Layer.Models
{
    public class PaginationExamEntity
    {
        public IEnumerable<ExamEntity> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
