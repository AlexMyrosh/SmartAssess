using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.AutoMapperProfiles.CustomResolvers;
using Presentation_Layer.ViewModels.Account;
using Presentation_Layer.ViewModels.Course;
using Presentation_Layer.ViewModels.ExamAssessment;

namespace Presentation_Layer.AutoMapperProfiles
{
    public class PresentationLayerAndBusinessLayerModelsMapper : Profile
    {
        public PresentationLayerAndBusinessLayerModelsMapper()
        {
            CreateMap<RegisterViewModel, UserModel>().ReverseMap();
            CreateMap<UserModel, AccountDetailsViewModel>().ReverseMap();
            CreateMap<ViewModels.Account.Shared.CourseViewModel, CourseModel>().ReverseMap();
            CreateMap<CourseModel, ViewModels.Course.Shared.CourseViewModel>().ReverseMap();
            CreateMap<UserModel, ViewModels.Course.Shared.UserViewModel>().ReverseMap();
            CreateMap<ViewModels.Course.Shared.ExamViewModel, ExamModel>();
            CreateMap<ExamModel, ViewModels.Course.Shared.ExamViewModel>()
                .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.Questions!.Count));

            CreateMap<PaginationCourseModel, AllCoursesViewModel>().ConvertUsing<PaginationCourseModelToAllCoursesViewModelConverter>();
            CreateMap<PaginationCourseModel, AppliedCoursesViewModel>().ConvertUsing<PaginationCourseModelToAppliedCoursesViewModelConverter>();
            CreateMap<PaginationCourseModel, CourseListWithPaginationViewModel>().ConvertUsing<PaginationCourseModelToCourseListWithPaginationViewModelConverter>();
            CreateMap<CourseModel, CourseDetailsViewModel>().ConvertUsing<CourseModelToCourseDetailsViewModelConverter>();
            CreateMap<UserExamAttemptModel, TakeExamViewModel>().ConvertUsing<UserExamAttemptModelToTakeExamViewModelConverter>();
            CreateMap<List<CourseModel>, MyPassedExamsViewModel>().ConvertUsing<ListCourseModelsToMyPassedExamsViewModelConverter>();

            CreateMap<TakeExamViewModel, UserExamAttemptModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AttemptId))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.QuestionAnswers));

            CreateMap<UserAnswerModel, ViewModels.ExamAssessment.Shared.QuestionAnswerViewModel>()
                .ForMember(dest => dest.QuestionAnswerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.QuestionMaxGrade, opt => opt.MapFrom(src => src.Question.MaxGrade))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.QuestionAnswerFeedback, opt => opt.MapFrom(src => src.Feedback));

            CreateMap<ViewModels.ExamAssessment.Shared.QuestionAnswerViewModel, UserAnswerModel>()
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuestionAnswerId));

            CreateMap<UserExamAttemptModel, TakenExamDetailsViewModel>()
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Name))
                .ForMember(dest => dest.ExamDescription, opt => opt.MapFrom(src => src.Exam.Description))
                .ForMember(dest => dest.IsExamAssessed, opt => opt.MapFrom(src => src.IsExamAssessed))
                .ForMember(dest => dest.IsAssessedByAi, opt => opt.MapFrom(src => src.IsAssessedByAi))
                .ForMember(dest => dest.TotalGrade, opt => opt.MapFrom(src => src.TotalGrade))
                .ForMember(dest => dest.MaxPossibleExamGrade, opt => opt.MapFrom(src => src.Exam.Questions.Sum(q=>q.MaxGrade)))
                .ForMember(dest => dest.TakenTimeToComplete, opt => opt.MapFrom(src => src.TakenTimeToComplete))
                .ForMember(dest => dest.MinExamPassGrade, opt => opt.MapFrom(src => src.Exam.MinimumPassGrade))
                .ForMember(dest => dest.GeneralFeedback, opt => opt.MapFrom(src => src.Feedback))
                .ForMember(dest => dest.QuestionAnswers, opt => opt.MapFrom(src => src.UserAnswers));
            
        }
    }
}