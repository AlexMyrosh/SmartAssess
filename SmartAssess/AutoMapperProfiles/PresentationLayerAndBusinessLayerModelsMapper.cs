using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.AutoMapperProfiles
{
    public class PresentationLayerAndBusinessLayerModelsMapper : Profile
    {
        public PresentationLayerAndBusinessLayerModelsMapper()
        {
            CreateMap<ExamViewModel, ExamModel>().ReverseMap();
            CreateMap<ExamQuestionViewModel, ExamQuestionModel>().ReverseMap();
            CreateMap<QuestionOptionViewModel, QuestionOptionModel>().ReverseMap();
        }
    }
}