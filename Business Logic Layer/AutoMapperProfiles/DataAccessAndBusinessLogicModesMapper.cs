using AutoMapper;
using Business_Logic_Layer.Models;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.AutoMapperProfiles
{
    public class DataAccessAndBusinessLogicModesMapper : Profile
    {
        public DataAccessAndBusinessLogicModesMapper()
        {
            CreateMap<ExamQuestionModel, ExamQuestionEntity>().ReverseMap();
            CreateMap<UserAnswerModel, UserAnswerEntity>().ReverseMap();
            CreateMap<UserModel, UserEntity>().ReverseMap();

            CreateMap<UserExamAttemptModel, UserExamAttemptEntity>()
                .ForMember(dest => dest.Exam, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UserExamAttemptEntity, UserExamAttemptModel>()
                .ForMember(dest => dest.TotalGrade, opt => opt.MapFrom(src => src.UserAnswers.Sum(ua => ua.Grade)));

            CreateMap<UserEntity, UserEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ExamModel, ExamEntity>();
            CreateMap<ExamEntity, ExamModel>()
                .ForMember(dest => dest.ExamDuration,
                    opt => opt.MapFrom(src => DateTime.Now + src.ExamDuration > src.ExamEndDateTime
                        ? src.ExamEndDateTime - DateTime.Now
                        : src.ExamDuration));
        }
    }
}
