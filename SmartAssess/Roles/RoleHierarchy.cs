namespace Presentation_Layer.Roles
{
    public static class RoleHierarchy
    {
        public const string Admin = Roles.Admin; // Administrator
        public const string Teacher = Admin + ", " + Roles.Teacher; // Administrator, Teacher
        public const string Student = Teacher + ", " + Roles.Student; // Administrator, Teacher, Student
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
    }

}
