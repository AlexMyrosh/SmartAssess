using Business_Logic_Layer.Enums;

namespace Business_Logic_Layer.Models
{
    public class ExamModel
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public SubjectModel? Subject { get; set; }

        public DateTimeOffset? ExamStartDateTime { get; set; }

        public DateTimeOffset? ExamEndDateTime { get; set; }

        public TimeSpan? ExamDuration { get; set; }

        public bool? IsAssessedByAi { get; set; }

        public int? MinimumPassGrade { get; set; }

        public bool? IsDeleted { get; set; }

        public List<ExamQuestionModel>? Questions { get; set; }

        public List<UserExamAttemptModel>? UserExamAttempts { get; set; }
    }
}
