using Presentation_Layer.ViewModels.Account.Shared;

namespace Presentation_Layer.ViewModels.Account
{
    public class AccountDetailsViewModel
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? EducationalInstitution { get; set; }

        public string? AboutUser { get; set; }

        public string? ImagePath { get; set; }

        public List<CourseViewModel> Courses { get; set; }
    }
}
