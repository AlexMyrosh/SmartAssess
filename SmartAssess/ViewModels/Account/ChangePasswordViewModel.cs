using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string? UserId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string? ConfirmNewPassword { get; set; }
    }
}
