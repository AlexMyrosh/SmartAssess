﻿using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.ViewModels.Exam
{
    public class ExamDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxAttemptsAllowed { get; set; }

        public DateTimeOffset ExamStartDateTime { get; set; }

        public DateTimeOffset ExamEndDateTime { get; set; }

        public TimeSpan ExamDuration { get; set; }

        public int MinimumPassGrade { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
    }
}