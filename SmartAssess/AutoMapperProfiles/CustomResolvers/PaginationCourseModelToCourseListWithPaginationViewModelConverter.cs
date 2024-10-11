using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.ViewModels.Course;
using Presentation_Layer.ViewModels.Course.Shared;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class PaginationCourseModelToCourseListWithPaginationViewModelConverter : ITypeConverter<PaginationCourseModel, CourseListWithPaginationViewModel>
{
    public CourseListWithPaginationViewModel Convert(PaginationCourseModel source, CourseListWithPaginationViewModel destination, ResolutionContext context)
    {
        var result = new CourseListWithPaginationViewModel
        {
            Pagination = new PaginationViewModel
            {
                TotalCourses = source.TotalItems
            },
            Courses = context.Mapper.Map<List<CourseViewModel>>(source.Items),
        };

        return result;
    }
}