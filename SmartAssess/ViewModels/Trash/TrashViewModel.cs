namespace Presentation_Layer.ViewModels.Trash
{
    public class TrashViewModel
    {
        public DeletedCourseListWithPaginationViewModel DeletedCoursesWithPagination { get; set; }

        public DeletedExamListWithPaginationViewModel DeletedExamsWithPagination { get; set; }

        public DeletedUserListWithPaginationViewModel DeletedUsersWithPagination { get; set; }
    }
}