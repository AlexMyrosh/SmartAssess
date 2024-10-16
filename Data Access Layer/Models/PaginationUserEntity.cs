namespace Data_Access_Layer.Models
{
    public class PaginationUserEntity
    {
        public IEnumerable<UserEntity> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
