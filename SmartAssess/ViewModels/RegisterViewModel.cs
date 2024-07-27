using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
    public class RegisterViewModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
