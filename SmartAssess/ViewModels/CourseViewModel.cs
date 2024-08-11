﻿namespace Presentation_Layer.ViewModels
{
    public class CourseViewModel
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<ExamViewModel>? Exams { get; set; }

        public List<UserViewModel>? Users { get; set; }
    }
}