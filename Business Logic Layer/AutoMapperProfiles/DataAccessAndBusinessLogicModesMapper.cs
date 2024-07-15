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
        }
    }
}
