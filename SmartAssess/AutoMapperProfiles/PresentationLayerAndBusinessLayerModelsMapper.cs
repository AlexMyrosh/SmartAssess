using AutoMapper;
using Business_Logic_Layer.Models;
using Data_Access_Layer.Models;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.AutoMapperProfiles
{
    public class PresentationLayerAndBusinessLayerModelsMapper : Profile
    {
        public PresentationLayerAndBusinessLayerModelsMapper()
        {
            CreateMap<PaginationCourseModel, PaginationCourseViewModel>().ReverseMap();
            CreateMap<ExamViewModel, ExamModel>().ReverseMap();
            CreateMap<ExamQuestionViewModel, ExamQuestionModel>().ReverseMap();
            CreateMap<UserAnswerViewModel, UserAnswerModel>().ReverseMap();
            CreateMap<RegisterViewModel, UserEntity>().ReverseMap();
            CreateMap<LoginViewModel, UserEntity>().ReverseMap();
            CreateMap<UserViewModel, UserEntity>().ReverseMap();
            CreateMap<UserViewModel, UserModel>().ReverseMap();
            CreateMap<CourseViewModel, CourseModel>().ReverseMap();

            CreateMap<ExamQuestionModel, UserAnswerViewModel>()
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src));

            CreateMap<ExamModel, UserExamAttemptViewModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.MapFrom(exam => exam))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.Questions));

            CreateMap<UserExamAttemptViewModel, UserExamAttemptModel>()
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.UserAnswers.Select(sa => new UserAnswerModel
                 {
                     Id = sa.Id,
                     AnswerText = sa.AnswerText,
                     QuestionId = sa.Question != null ? sa.Question.Id : null,
                    Grade = sa.Grade
                 }).ToList()));

            CreateMap<UserExamAttemptModel, UserExamAttemptViewModel>()
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.UserAnswers.Select(ua => new UserAnswerViewModel
                {
                    Id = ua.Id,
                    AnswerText = ua.AnswerText,
                    Grade = ua.Grade,
                    Feedback = ua.Feedback,
                    Question = new ExamQuestionViewModel
                    {
                        QuestionText = ua.Question.QuestionText
                    }
                })));
        }
    }
}