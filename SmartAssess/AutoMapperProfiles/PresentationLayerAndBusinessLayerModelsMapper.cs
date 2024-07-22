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
            CreateMap<ExamViewModel, ExamModel>().ReverseMap();
            CreateMap<ExamQuestionViewModel, ExamQuestionModel>().ReverseMap();
            CreateMap<TeacherNoteViewModel, TeacherNoteModel>().ReverseMap();
            CreateMap<UserAnswerViewModel, UserAnswerModel>().ReverseMap();

            CreateMap<RegisterViewModel, UserEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<LoginViewModel, UserEntity>().ReverseMap();

            CreateMap<ExamModel, UserExamPassViewModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ExamDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ExamSubject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.ExamId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.Questions.Select(q => new UserAnswerViewModel
                {
                    QuestionText = q.QuestionText,
                    QuestionId = q.Id
                })));

            CreateMap<UserExamPassViewModel, UserExamPassModel>()
                .ForMember(dest => dest.ExamId, opt => opt.MapFrom(src => src.ExamId))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.UserAnswers.Select(sa => new UserAnswerModel
                {
                    Id = sa.Id,
                    Answer = sa.AnswerText,
                    QuestionId = sa.QuestionId,
                    Grade = sa.Grade ?? 0
                }).ToList()));

            CreateMap<UserExamPassModel, UserExamPassViewModel>()
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.UserAnswers.Select(ua => new UserAnswerViewModel
                {
                    Id = ua.Id,
                    AnswerText = ua.Answer,
                    QuestionText = ua.Question.QuestionText,
                    Grade = ua.Grade
                })));
        }
    }
}