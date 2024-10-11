using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels.Course;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class PaginationCourseModelToAppliedCoursesViewModelConverter : ITypeConverter<PaginationCourseModel, AppliedCoursesViewModel>
{
    public AppliedCoursesViewModel Convert(PaginationCourseModel source, AppliedCoursesViewModel destination, ResolutionContext context)
    {
        var result = new AppliedCoursesViewModel
        {
            CourseListWithPagination = context.Mapper.Map<CourseListWithPaginationViewModel>(source),
            CurrentUserRole = source.CurrentUserRole
        };

        return result;
    }
}