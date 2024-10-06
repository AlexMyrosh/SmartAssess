using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace Presentation_Layer.ViewModels.ExamAssessment
{
    public class TakeExamViewModel
    {
        public Guid AttemptId { get; set; }
        public string ExamName { get; set; }

        public string CourseName { get; set; }

        public string ExamDescription { get; set; }

        public int ExamMaxAttemptsAllowed { get; set; }

        public int UserAttemptCount { get; set; }

        public int ExamMinPassGrade { get; set; }

        public int MaxPossibleExamGrade { get; set; }

        public TimeSpan ExamDuration { get; set; }

        public TimeSpan TakenTimeToComplete { get; set; }

        public List<QuestionAnswerViewModel> QuestionAnswers { get; set; }
    }
}