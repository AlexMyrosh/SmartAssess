using Business_Logic_Layer.Models;

namespace Presentation_Layer.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? EducationalInstitution { get; set; }

        public string? AboutUser { get; set; }

        public string? Interests { get; set; }

        public DateTime? LastLogInDateTime { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<CourseViewModel> Courses { get; set; }
    }
}
