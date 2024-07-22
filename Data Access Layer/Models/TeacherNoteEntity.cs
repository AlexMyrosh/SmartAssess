﻿using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Models
{
    public class TeacherNoteEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        public int? MinGrade { get; set; }

        public int? MaxGrade { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        public virtual ExamQuestionEntity Question { get; set; }
    }
}