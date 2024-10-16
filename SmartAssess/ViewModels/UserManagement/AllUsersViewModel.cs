using Presentation_Layer.ViewModels.UserManagement.Shared;

namespace Presentation_Layer.ViewModels.UserManagement
{
    public class AllUsersViewModel
    {
        public List<UserViewModel> Users { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
