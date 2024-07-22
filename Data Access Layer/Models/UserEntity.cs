using Microsoft.AspNetCore.Identity;

namespace Data_Access_Layer.Models
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ClassGroup { get; set; }

        public List<UserExamPassEntity> UserExamPasses { get; set; }
    }
}