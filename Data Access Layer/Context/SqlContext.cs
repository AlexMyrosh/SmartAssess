using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Context
{
    public class SqlContext : IdentityDbContext<UserEntity>
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<ExamEntity> Exams { get; set; }

        public DbSet<ExamQuestionEntity> ExamQuestions { get; set; }

        public DbSet<UserAnswerEntity> UserAnswers { get; set; }

        public DbSet<UserExamAttemptEntity> UserExamAttempts { get; set; }

        public DbSet<CourseEntity> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ExamQuestionEntity
            modelBuilder.Entity<ExamQuestionEntity>()
                .HasOne(question => question.Exam)
                .WithMany(exam => exam.Questions)
                .HasForeignKey(question => question.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure UserExamAttemptEntity
            modelBuilder.Entity<UserExamAttemptEntity>()
                .HasOne(attempt => attempt.User)
                .WithMany(user => user.UserExamAttempts)
                .HasForeignKey(attempt => attempt.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserExamAttemptEntity>()
                .HasOne(attempt => attempt.Exam)
                .WithMany(exam => exam.UserExamAttempts)
                .HasForeignKey(attempt => attempt.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure UserAnswerEntity
            modelBuilder.Entity<UserAnswerEntity>()
                .HasOne(answer => answer.Question)
                .WithMany(question => question.UserAnswers)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAnswerEntity>()
                .HasOne(answer => answer.UserExamAttempt)
                .WithMany(pass => pass.UserAnswers)
                .HasForeignKey(answer => answer.UserExamAttemptId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure ExamEntity
            modelBuilder.Entity<ExamEntity>()
                .HasOne(answer => answer.Course)
                .WithMany(pass => pass.Exams)
                .HasForeignKey(answer => answer.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure CourseEntity
            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Users)
                .WithMany(u => u.Courses)
                .UsingEntity(j => j.ToTable("CourseUsers"));

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Teachers)
                .WithMany(t => t.TeachingCourses)
                .UsingEntity(j => j.ToTable("CourseTeachers"));
        }
    }
}