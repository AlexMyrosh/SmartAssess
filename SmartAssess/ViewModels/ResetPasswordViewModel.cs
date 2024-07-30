using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        public string Code { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set;}
    }
}
