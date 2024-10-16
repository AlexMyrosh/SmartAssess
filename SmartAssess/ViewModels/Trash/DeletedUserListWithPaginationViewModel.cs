using Presentation_Layer.ViewModels.Trash.Shared;

namespace Presentation_Layer.ViewModels.Trash
{
    public class DeletedUserListWithPaginationViewModel
    {
        public string SearchQuery { get; set; }

        public List<UserViewModel> Users { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}