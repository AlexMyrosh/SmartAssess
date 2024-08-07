﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class UserExamAttemptEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(1000)]
        public string? Feedback { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }

        [Required]
        public Guid ExamId { get; set; }
        public virtual ExamEntity Exam { get; set; }

        public virtual List<UserAnswerEntity> UserAnswers { get; set; }
    }
}