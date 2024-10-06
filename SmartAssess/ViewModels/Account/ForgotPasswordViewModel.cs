using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
