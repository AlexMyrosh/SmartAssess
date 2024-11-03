using AutoMapper;
using Business_Logic_Layer.Models;
using ViewModels.Enums;
using Presentation_Layer.ViewModels.Exam;
using Presentation_Layer.ViewModels.Exam.Shared;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class ExamModelToExamResultViewModelConverter : ITypeConverter<ExamModel, ExamResultViewModel>
{
    public ExamResultViewModel Convert(ExamModel source, ExamResultViewModel destination, ResolutionContext context)
    {
        var result = new ExamResultViewModel
        {
            ExamName = source.Name,
            MaxPossibleExamGrade = source.Questions.Sum(q=>q.MaxGrade).Value,
            MinimumPassGrade = source.MinimumPassGrade.Value,
        };

        result.UserAttempts = source.UserExamAttempts
            .GroupBy(attempt => attempt.User!.Id)
            .ToDictionary(
                group => context.Mapper.Map<UserViewModel>(source.UserExamAttempts.First(x => x.User!.Id == group.Key).User),
                group => new ExamAttemptsViewModel
                {
                    UserAttempts = group.Select(attempt => context.Mapper.Map<ExamAttemptViewModel>(attempt)).ToList()
                }
            );

        foreach(var userAttempts in result.UserAttempts)
        {
            userAttempts.Value.FinalGrade = CalculateFinalGrade(userAttempts.Value.UserAttempts, (FinalGradeCalculationMethodViewModel)source.FinalGradeCalculationMethod);
        }

        return result;
    }

    private double? CalculateFinalGrade(List<ExamAttemptViewModel> examAttempts, FinalGradeCalculationMethodViewModel calculationMethod)
    {
        double? grade = null;
        var assessedAttempts = examAttempts.Where(a => a.IsExamAssessed);
        if(assessedAttempts.Any() )
        {
            if (calculationMethod == FinalGradeCalculationMethodViewModel.Average)
            {
                grade = assessedAttempts.Average(x => x.TotalGrade);
            }
            else if (calculationMethod == FinalGradeCalculationMethodViewModel.Max)
            {
                grade = assessedAttempts.Max(x => x.TotalGrade);
            }
            else if (calculationMethod == FinalGradeCalculationMethodViewModel.LastAttempt)
            {
                grade = assessedAttempts.MinBy(x => x.AttemptStarterAt)?.TotalGrade ?? null;
            }
        }

        return grade;
    }
}