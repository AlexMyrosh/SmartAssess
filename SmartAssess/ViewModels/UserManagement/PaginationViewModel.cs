namespace Presentation_Layer.ViewModels.UserManagement
{
    public class PaginationViewModel
    {
        public int TotalItems { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
}
