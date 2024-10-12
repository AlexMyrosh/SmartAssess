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
        ExamStatisticViewModel result;

        if (source.UserExamAttempts.Count > 0)
        {
            var examAttemptsByUsersDistribution = source.UserExamAttempts
                .GroupBy(attempt => attempt.User.Id)
                .ToDictionary(
                    group => source.UserExamAttempts.Where(x => x.User.Id == group.Key).FirstOrDefault().User,
                    group => group.Select(attempt => context.Mapper.Map<UserExamAttemptViewModel>(attempt)).ToList()
                );

            var finalUserGradeDistribution = new Dictionary<UserModel, double>(examAttemptsByUsersDistribution.Count);
            if (source.FinalGradeCalculationMethod == FinalGradeCalculationMethodModel.Average)
            {
                foreach (var examAttempt in examAttemptsByUsersDistribution)
                {
                    var finalUserAttemptGrade = examAttempt.Value.Where(a => a.IsAssessed).Average(x => x.TotalGrade);
                    if (finalUserAttemptGrade is not null)
                    {
                        finalUserGradeDistribution.Add(examAttempt.Key, finalUserAttemptGrade.Value);
                    }
                }
            }
            else if (source.FinalGradeCalculationMethod == FinalGradeCalculationMethodModel.Max)
            {
                foreach (var examAttempt in examAttemptsByUsersDistribution)
                {
                    var finalUserAttemptGrade = examAttempt.Value.Where(a => a.IsAssessed).Max(x => x.TotalGrade);
                    if (finalUserAttemptGrade is not null)
                    {
                        finalUserGradeDistribution.Add(examAttempt.Key, finalUserAttemptGrade.Value);
                    }
                }
            }
            else if (source.FinalGradeCalculationMethod == FinalGradeCalculationMethodModel.LastAttempt)
            {
                foreach (var examAttempt in examAttemptsByUsersDistribution)
                {
                    var finalUserAttemptGrade = examAttempt.Value.Where(a => a.IsAssessed).OrderBy(x => x.AttemptStarterAt).FirstOrDefault().TotalGrade;
                    if (finalUserAttemptGrade is not null)
                    {
                        finalUserGradeDistribution.Add(examAttempt.Key, finalUserAttemptGrade.Value);
                    }
                }
            }

            result = new ExamStatisticViewModel
            {
                ExamTakerCount = examAttemptsByUsersDistribution.Count,
                MaxPossibleExamGrade = source.Questions.Sum(q => q.MaxGrade.Value),
                ExamAssessedByAiNumberOfUserAttempts = source.UserExamAttempts.Count(x => x.IsExamAssessed && x.IsAssessedByAi),
                ExamAssessedManuallyNumberOfUserAttempts = source.UserExamAttempts.Count(x => x.IsExamAssessed && !x.IsAssessedByAi),
                WaitingForAssessmentNumberOfUserAttempts = source.UserExamAttempts.Count(x => !x.IsExamAssessed),
                NumberOfUsersWithNoneOfCheckedAttempts = examAttemptsByUsersDistribution.Count(x => x.Value.All(y => !y.IsAssessed)),
                AverageTimeSpentToCompleteExam = TimeSpan.FromTicks((long)source.UserExamAttempts.Average(x => x.TakenTimeToComplete.Ticks)),
                IsExamTakenByStudents = source.UserExamAttempts.Count > 0,
                IsExamHasAssessedUserAttempts = source.UserExamAttempts.Count(x => x.IsExamAssessed) > 0,
                AssessedUserAttemptsFromMinToMaxGrade = new List<UserExamAttemptViewModel>(),
                AssessedUserAttemptsFromMaxToMinGrade = new List<UserExamAttemptViewModel>()
            };

            if (finalUserGradeDistribution != null && finalUserGradeDistribution.Count > 0)
            {
                result.ExamTakerAverageGrade = finalUserGradeDistribution.Average(x => x.Value);
                result.PassedExamNumberOfUsers = finalUserGradeDistribution.Count(x => x.Value >= source.MinimumPassGrade);
                result.FailedExamNumberOfUsers = finalUserGradeDistribution.Count(x => x.Value < source.MinimumPassGrade);
            }

            foreach (var item in finalUserGradeDistribution.OrderBy(x => x.Value))
            {
                result.AssessedUserAttemptsFromMinToMaxGrade.Add(new UserExamAttemptViewModel
                {
                    UserFirstName = item.Key.FirstName,
                    UserLastName = item.Key.LastName,
                    TotalGrade = item.Value
                });
            }

            foreach (var item in finalUserGradeDistribution.OrderByDescending(x => x.Value))
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
            foreach (var item in finalUserGradeDistribution)
            {
                var categoryIndex = (int)item.Value / categoryStep;
                result.UserGradeDistribution[categoryIndex]++;
            }
        }
        else
        {
            result = new ExamStatisticViewModel
            {
                IsExamTakenByStudents = false,
                IsExamHasAssessedUserAttempts = false
            };
        }

        

        return result;
    }
}