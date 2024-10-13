using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using Presentation_Layer.ViewModels.Exam;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class ExamModelToExamStatisticViewModelConverter : ITypeConverter<ExamModel, ExamStatisticViewModel>
{
    public ExamStatisticViewModel Convert(ExamModel source, ExamStatisticViewModel destination, ResolutionContext context)
    {
        var result = new ExamStatisticViewModel();
        if (source.UserExamAttempts is null)
        {
            return result;
        }

        var examAttemptsByUsersDistribution = source.UserExamAttempts
            .GroupBy(attempt => attempt.User!.Id)
            .ToDictionary(
                group => source.UserExamAttempts.First(x => x.User!.Id == group.Key).User,
                group => group.Select(attempt => context.Mapper.Map<UserExamAttemptViewModel>(attempt)).ToList()
            );

        var finalAttemptGradeByUserDistribution = new Dictionary<UserModel, double>(examAttemptsByUsersDistribution.Count);
        foreach (var examAttemptByUsers in examAttemptsByUsersDistribution)
        {
            if (examAttemptByUsers.Key is null || examAttemptByUsers.Value.Count == 0)
            {
                continue;
            }

            double? grade = 0;
            if (source.FinalGradeCalculationMethod == FinalGradeCalculationMethodModel.Average)
            {
                grade = examAttemptByUsers.Value.Where(a => a.IsAssessed).Average(x => x.TotalGrade);
            }
            else if (source.FinalGradeCalculationMethod == FinalGradeCalculationMethodModel.Max)
            {
                grade = examAttemptByUsers.Value.Where(a => a.IsAssessed).Max(x => x.TotalGrade);
            }
            else if (source.FinalGradeCalculationMethod == FinalGradeCalculationMethodModel.LastAttempt)
            {
                grade = examAttemptByUsers.Value.Where(a => a.IsAssessed).MinBy(x => x.AttemptStarterAt)?.TotalGrade;
            }

            if (grade is not null)
            {
                finalAttemptGradeByUserDistribution.Add(examAttemptByUsers.Key, grade.Value);
            }
        }

        result.ExamTakerCount = examAttemptsByUsersDistribution.Count;
        if (source.Questions != null)
        {
            result.MaxPossibleExamGrade = source.Questions.Sum(q => q.MaxGrade!.Value);
        }

        result.ExamAssessedByAiNumberOfUserAttempts = source.UserExamAttempts.Count(x => x is { IsExamAssessed: true, IsAssessedByAi: true });
        result.ExamAssessedManuallyNumberOfUserAttempts = source.UserExamAttempts.Count(x => x is { IsExamAssessed: true, IsAssessedByAi: false });
        result.WaitingForAssessmentNumberOfUserAttempts = source.UserExamAttempts.Count(x => !x.IsExamAssessed);
        result.NumberOfUsersWithNoneOfCheckedAttempts = examAttemptsByUsersDistribution.Count(x => x.Value.All(y => !y.IsAssessed));
        result.IsExamTakenByStudents = source.UserExamAttempts.Count > 0;
        result.IsExamHasAssessedUserAttempts = source.UserExamAttempts.Count(x => x.IsExamAssessed) > 0;
        result.AverageTimeSpentToCompleteExam = source.UserExamAttempts.Any() ? TimeSpan.FromTicks((long)source.UserExamAttempts.Average(x => x.TakenTimeToComplete.Ticks)) : TimeSpan.Zero;
        result.AssessedUserAttemptsFromMinToMaxGrade = new List<UserExamAttemptViewModel>();
        result.AssessedUserAttemptsFromMaxToMinGrade = new List<UserExamAttemptViewModel>();

        if (finalAttemptGradeByUserDistribution.Count > 0)
        {
            result.ExamTakerAverageGrade = finalAttemptGradeByUserDistribution.Average(x => x.Value);
            result.PassedExamNumberOfUsers = finalAttemptGradeByUserDistribution.Count(x => x.Value >= source.MinimumPassGrade);
            result.FailedExamNumberOfUsers = finalAttemptGradeByUserDistribution.Count(x => x.Value < source.MinimumPassGrade);
        }

        foreach (var item in finalAttemptGradeByUserDistribution.OrderBy(x => x.Value))
        {
            result.AssessedUserAttemptsFromMinToMaxGrade.Add(new UserExamAttemptViewModel
            {
                UserFirstName = item.Key.FirstName,
                UserLastName = item.Key.LastName,
                TotalGrade = item.Value
            });
        }

        foreach (var item in finalAttemptGradeByUserDistribution.OrderByDescending(x => x.Value))
        {
            result.AssessedUserAttemptsFromMaxToMinGrade.Add(new UserExamAttemptViewModel
            {
                UserFirstName = item.Key.FirstName,
                UserLastName = item.Key.LastName,
                TotalGrade = item.Value
            });
        }

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
        foreach (var item in finalAttemptGradeByUserDistribution)
        {
            var categoryIndex = (int)item.Value / categoryStep;
            result.UserGradeDistribution[categoryIndex]++;
        }

        return result;
    }
}