namespace Business_Logic_Layer.Models
{
    public class CourseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LongDescription { get; set; }

        public List<ExamModel> Exams { get; set; }

        public List<UserModel> Users { get; set; }

        public List<UserModel> Teachers { get; set; }
    }
}
