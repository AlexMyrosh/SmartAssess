﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class CourseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public virtual List<ExamEntity> Exams { get; set; }

        public virtual List<UserEntity> Users { get; set; }
    }
}