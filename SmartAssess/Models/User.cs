using Microsoft.AspNetCore.Identity;

namespace Presentation_Layer.Models
{
    public class User : IdentityUser
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
