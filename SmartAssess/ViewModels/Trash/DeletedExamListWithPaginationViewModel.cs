using Presentation_Layer.ViewModels.Trash.Shared;

namespace Presentation_Layer.ViewModels.Trash
{
    public class DeletedExamListWithPaginationViewModel
    {
        public string SearchQuery { get; set; }

        public List<ExamViewModel> Exams { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}