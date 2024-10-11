using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels.Course;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class PaginationCourseModelToAllCoursesViewModelConverter : ITypeConverter<PaginationCourseModel, AllCoursesViewModel>
{
    public AllCoursesViewModel Convert(PaginationCourseModel source, AllCoursesViewModel destination, ResolutionContext context)
    {
        var result = new AllCoursesViewModel
        {
            CourseListWithPagination = context.Mapper.Map<CourseListWithPaginationViewModel>(source)
        };

        return result;
    }
}