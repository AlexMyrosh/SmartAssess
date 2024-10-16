using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Models
{
    public class UserEntity : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? EducationalInstitution { get; set; }

        [MaxLength(5000)]
        public string? AboutUser { get; set; }

        public string? ImagePath { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }

        public string? DeletedById { get; set; }

        public virtual UserEntity? DeletedBy { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public virtual List<UserExamAttemptEntity> UserExamAttempts { get; set; }

        public virtual List<CourseEntity> Courses { get; set; }

        public virtual List<CourseEntity> TeachingCourses { get; set; }

        public virtual List<CourseEntity> DeletedCourses { get; set; }

        public virtual List<ExamEntity> DeletedExams { get; set; }

        public virtual List<UserEntity> DeletedUsers { get; set; }
    }
}