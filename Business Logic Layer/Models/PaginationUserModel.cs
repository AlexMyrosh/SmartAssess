namespace Business_Logic_Layer.Models
{
    public class PaginationUserModel
    {
        public List<UserModel> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
