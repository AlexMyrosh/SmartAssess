namespace Business_Logic_Layer.Models
{
    public class CourseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LongDescription { get; set; }

        public bool IsCurrentUserApplied { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }

        public UserModel? DeletedBy { get; set; }

        public List<ExamModel> Exams { get; set; }

        public List<UserModel> Users { get; set; }

        public List<UserModel> Teachers { get; set; }
    }
}
