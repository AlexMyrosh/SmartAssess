using Presentation_Layer.Enums;

namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public SubjectViewModel? Subject { get; set; }

        public DateTime? ExamStartDateTime { get; set; }

        public DateTime? ExamEndDateTime { get; set; }

        public TimeSpan? ExamDuration { get; set; }

        public bool IsAssessedByAi { get; set; }

        public int MinimumPassGrade { get; set; }

        public bool? IsDeleted { get; set; }

        public List<ExamQuestionViewModel>? Questions { get; set; }
    }
}
