using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
