using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
    public class RegisterViewModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Remote("ValidateUsername", "Account")]
        public string? UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Remote("ValidateEmail", "Account")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
