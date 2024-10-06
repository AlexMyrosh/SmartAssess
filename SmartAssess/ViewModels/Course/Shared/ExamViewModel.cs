using Presentation_Layer.ViewModels.Enums;

namespace Presentation_Layer.ViewModels.Course.Shared
{
    public class ExamViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxAttemptsAllowed { get; set; }

        public DateTimeOffset ExamStartDateTime { get; set; }

        public DateTimeOffset ExamEndDateTime { get; set; }

        public TimeSpan ExamDuration { get; set; }

        public int MinimumPassGrade { get; set; }

        public FinalGradeCalculationMethodViewModel FinalGradeCalculationMethod { get; set; }

        public int UserAttemptCount { get; set; }

        public Guid? StartedAttemptId { get; set; }

        public int NumberOfQuestions { get; set; }
    }
}
