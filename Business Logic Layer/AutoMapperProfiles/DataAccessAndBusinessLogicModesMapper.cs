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
            CreateMap<TeacherNoteModel, TeacherNoteEntity>().ReverseMap();
            CreateMap<UserAnswerEntity, UserAnswerModel>().ReverseMap();

            CreateMap<UserExamPassEntity, UserExamPassModel>()
                .ForMember(dest => dest.TotalGrade, opt => opt.MapFrom(src => src.UserAnswers.Sum(ua => ua.Grade)));

            CreateMap<UserExamPassModel, UserExamPassEntity>();
        }
    }
}
