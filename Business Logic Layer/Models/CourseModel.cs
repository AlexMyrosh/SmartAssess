namespace Business_Logic_Layer.Models
{
    public class CourseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ExamModel> Exams { get; set; }

        public List<UserModel> Users { get; set; }
    }
}
