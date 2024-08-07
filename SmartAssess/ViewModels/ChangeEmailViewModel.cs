using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
    public class ChangeEmailViewModel
    {
        [EmailAddress]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }
    }
}