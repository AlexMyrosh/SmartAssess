using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels.Course;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class CourseModelToCourseDetailsViewModelConverter : ITypeConverter<CourseModel, CourseDetailsViewModel>
{
    public CourseDetailsViewModel Convert(CourseModel source, CourseDetailsViewModel destination, ResolutionContext context)
    {
        var result = new CourseDetailsViewModel
        {
            Id = source.Id,
            Name = source.Name,
            LongDescription = source.LongDescription,
            TeacherIds = source.Teachers.Select(teacher => teacher.Id).ToList(),
            IsCurrentUserApplied = source.IsCurrentUserApplied,
            AppliedUsers = context.Mapper.Map<List<ViewModels.Course.Shared.UserViewModel>>(source.Users),
            Exams = context.Mapper.Map<List<ViewModels.Course.Shared.ExamViewModel>>(source.Exams)
        };

        return result;
    }
}