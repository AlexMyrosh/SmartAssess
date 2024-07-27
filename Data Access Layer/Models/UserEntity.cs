using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class UserEntity : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string LastName { get; set; }

        public List<UserExamAttemptEntity> UserExamAttempts { get; set; }
    }
}