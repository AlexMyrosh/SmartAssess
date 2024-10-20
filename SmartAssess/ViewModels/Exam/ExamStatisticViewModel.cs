﻿using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.ViewModels.Exam
{
    public class ExamStatisticViewModel
    {
        public int ExamTakerCount { get; set; }

        public double ExamTakerAverageGrade { get; set; }

        public int MaxPossibleExamGrade { get; set; }

        public int PassedExamNumberOfUsers { get; set; }

        public int FailedExamNumberOfUsers { get; set; }

        public int ExamAssessedByAiNumberOfUserAttempts { get; set; }

        public int ExamAssessedManuallyNumberOfUserAttempts { get; set; }

        public int WaitingForAssessmentNumberOfUserAttempts { get; set; }

        public int NumberOfUsersWithNoneOfCheckedAttempts { get; set; }

        public TimeSpan AverageTimeSpentToCompleteExam { get; set; }

        public List<string> ExamGradeDistribution { get; set; }

        public int[] UserGradeDistribution { get; set; }

        public bool IsExamTakenByStudents { get; set; }

        public bool IsExamHasAssessedUserAttempts { get; set; }

        public List<UserExamAttemptViewModel> AssessedUserAttemptsFromMinToMaxGrade { get; set; }

        public List<UserExamAttemptViewModel> AssessedUserAttemptsFromMaxToMinGrade { get; set; }

        public List<UserExamAttemptViewModel> UserAttempts { get; set; }
    }
}