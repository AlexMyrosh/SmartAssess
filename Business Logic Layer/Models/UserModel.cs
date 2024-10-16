﻿namespace Business_Logic_Layer.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? EducationalInstitution { get; set; }

        public string? AboutUser { get; set; }

        public string? ImagePath { get; set; }

        public string Role { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }

        public UserModel? DeletedBy { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<CourseModel> Courses { get; set; }
    }
}