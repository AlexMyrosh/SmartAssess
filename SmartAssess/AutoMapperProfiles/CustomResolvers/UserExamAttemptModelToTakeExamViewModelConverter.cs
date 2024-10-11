using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels.ExamAssessment;
using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class UserExamAttemptModelToTakeExamViewModelConverter : ITypeConverter<UserExamAttemptModel, TakeExamViewModel>
{
    public TakeExamViewModel Convert(UserExamAttemptModel source, TakeExamViewModel destination, ResolutionContext context)
    {
        if (source.Exam is null || source.Exam.Course is null)
        {
            return null;
        }

        var result = new TakeExamViewModel
        {
            AttemptId = source.Id.Value,
            ExamName = source.Exam.Name,
            CourseName = source.Exam.Course.Name,
            ExamDescription = source.Exam.Description,
            ExamMaxAttemptsAllowed = source.Exam.MaxAttemptsAllowed.Value,
            UserAttemptCount = source.Exam.UserAttemptCount,
            ExamMinPassGrade = source.Exam.MinimumPassGrade.Value,
            MaxPossibleExamGrade = source.Exam.Questions.Sum(q => q.MaxGrade).Value,
            TakenTimeToComplete = source.TakenTimeToComplete,
            QuestionAnswers = context.Mapper.Map<List<QuestionAnswerViewModel>>(source.UserAnswers)
        };

        if (source.AttemptStarterAt != null && source.AttemptStarterAt != DateTimeOffset.MinValue)
        {
            result.RemainingTimeToCompleteExam = source.Exam.ExamDuration.Value - (DateTime.UtcNow - source.AttemptStarterAt.UtcDateTime);
        }
        else
        {
            result.RemainingTimeToCompleteExam = source.Exam.ExamDuration.Value;
        }

        if (source.Exam.ExamEndDateTime.Value.UtcDateTime - DateTime.UtcNow < source.Exam.ExamDuration)
        {
            result.RemainingTimeToCompleteExam = source.Exam.ExamEndDateTime.Value.UtcDateTime - DateTime.UtcNow;
        }

        return result;
    }
}