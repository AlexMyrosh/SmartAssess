namespace Presentation_Layer.ViewModels.Trash.Shared
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string DeletedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; }
    }
}
