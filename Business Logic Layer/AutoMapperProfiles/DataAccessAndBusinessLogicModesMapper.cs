using AutoMapper;
using Business_Logic_Layer.Models;
using Data_Access_Layer.Models;

namespace Business_Logic_Layer.AutoMapperProfiles
{
    public class DataAccessAndBusinessLogicModesMapper : Profile
    {
        public DataAccessAndBusinessLogicModesMapper()
        {
            CreateMap<ExamModel, ExamEntity>().ReverseMap();
            CreateMap<ExamQuestionModel, ExamQuestionEntity>().ReverseMap();
            CreateMap<UserAnswerEntity, UserAnswerModel>().ReverseMap();

            CreateMap<UserExamAttemptEntity, UserExamPassModel>()
                .ForMember(dest => dest.TotalGrade, opt => opt.MapFrom(src => src.UserAnswers.Sum(ua => ua.Grade)));

            CreateMap<UserExamPassModel, UserExamAttemptEntity>();
            CreateMap<UserEntity, UserEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
