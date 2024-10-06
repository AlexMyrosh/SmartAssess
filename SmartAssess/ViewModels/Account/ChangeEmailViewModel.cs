using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels.Account
{
    public class ChangeEmailViewModel
    {
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }
    }
}