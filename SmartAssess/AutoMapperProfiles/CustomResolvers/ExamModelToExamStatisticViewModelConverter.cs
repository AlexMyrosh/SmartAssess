using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels.Exam;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class ExamModelToExamStatisticViewModelConverter : ITypeConverter<ExamModel, ExamStatisticViewModel>
{
    public ExamStatisticViewModel Convert(ExamModel source, ExamStatisticViewModel destination, ResolutionContext context)
    {
        var result = new ExamStatisticViewModel
        {
            ExamTakerCount = source.UserExamAttempts.DistinctBy(x => x.User.Id).Count(),
            ExamTakerAverageGrade = source.UserExamAttempts.Where(a=>a.IsExamAssessed).Average(x=>x.TotalGrade.Value),
            MaxPossibleExamGrade = source.Questions.Sum(q=>q.MaxGrade.Value),
            PassedExamNumberOfUsers = source.UserExamAttempts.Where(x=> x.IsExamAssessed && x.TotalGrade >= source.MinimumPassGrade).DistinctBy(x=>x.User.Id).Count(),
            FailedExamNumberOfUsers = source.UserExamAttempts.Where(x => x.IsExamAssessed && x.TotalGrade < source.MinimumPassGrade).DistinctBy(x => x.User.Id).Count(),
            ExamAssessedByAiNumberOfUserAttempts = source.UserExamAttempts.Count(x=>x.IsExamAssessed && x.IsAssessedByAi),
            ExamAssessedManuallyNumberOfUserAttempts = source.UserExamAttempts.Count(x => x.IsExamAssessed && !x.IsAssessedByAi),
            WaitingForAssessmentNumberOfUserAttempts = source.UserExamAttempts.Count(x => !x.IsExamAssessed),
            AverageTimeSpentToCompleteExam = TimeSpan.FromTicks((long)source.UserExamAttempts.Average(x => x.TakenTimeToComplete.Ticks)),
            UserAttempts = context.Mapper.Map<List<UserExamAttemptViewModel>>(source.UserExamAttempts),
            AssessedUserAttempts = context.Mapper.Map<List<UserExamAttemptViewModel>>(source.UserExamAttempts.Where(x=>x.IsAssessedByAi))
        };

        var examMaxGrade = result.MaxPossibleExamGrade;
        var categoryCount = examMaxGrade switch
        {
            <= 5 => 2,
            <= 15 => 5,
            <= 30 => 6,
            <= 45 => 7,
            <= 60 => 8,
            _ => 10
        };

        var categoryStep = (int)Math.Ceiling((float)examMaxGrade / categoryCount);
        result.ExamGradeDistribution = Enumerable.Range(0, categoryCount)
            .Select(n => n == categoryCount - 1
                ? $"{n * categoryStep}-{examMaxGrade}"
                : $"{n * categoryStep}-{(n + 1) * categoryStep - 1}")
            .ToList();


        result.UserGradeDistribution = new int[categoryCount];
        foreach (var student in source.UserExamAttempts.Where(x => x.IsExamAssessed))
        {
            var categoryIndex = student.TotalGrade / categoryStep;
            result.UserGradeDistribution[categoryIndex.Value]++;
        }

        return result;
    }
}