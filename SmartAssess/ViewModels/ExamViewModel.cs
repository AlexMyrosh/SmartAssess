namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? MaxAttemptsAllowed { get; set; }

        public DateTimeOffset? ExamStartDateTime { get; set; }

        public DateTimeOffset? ExamEndDateTime { get; set; }

        public TimeSpan? ExamDuration { get; set; }

        public bool IsAssessedByAi { get; set; }

        public int MinimumPassGrade { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CurrentUserAttmptNumber { get; set; }

        public Guid? StartedByCurrentUserAttemptId { get; set; }

        public CourseViewModel? Course { get; set; }

        public List<ExamQuestionViewModel>? Questions { get; set; }

        public List<UserExamAttemptViewModel>? UserExamAttempts { get; set; }
    }
}
