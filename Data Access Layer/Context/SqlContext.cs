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
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict

            // Configure UserAnswerEntity
            modelBuilder.Entity<UserAnswerEntity>()
                .HasOne(answer => answer.Question)
                .WithMany(question => question.UserAnswers)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Ensure NoAction or Restrict

            modelBuilder.Entity<UserAnswerEntity>()
                .HasOne(answer => answer.UserExamAttempt)
                .WithMany(pass => pass.UserAnswers)
                .HasForeignKey(answer => answer.UserExamAttemptId)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict

            // Configure ExamEntity
            modelBuilder.Entity<ExamEntity>()
                .HasOne(answer => answer.Course)
                .WithMany(pass => pass.Exams)
                .HasForeignKey(answer => answer.CourseId)
                .OnDelete(DeleteBehavior.NoAction); // Ensure NoAction or Restrict

            // Configure CourseEntity
            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Users)
                .WithMany(u => u.Courses)
                .UsingEntity(j => j.ToTable("CourseUsers"));

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Teachers)
                .WithMany(t => t.TeachingCourses)
                .UsingEntity(j => j.ToTable("CourseTeachers"));

            modelBuilder.Entity<CourseEntity>()
                .HasOne(course => course.DeletedBy)
                .WithMany(user => user.DeletedCourses)
                .HasForeignKey(course => course.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExamEntity>()
                .HasOne(course => course.DeletedBy)
                .WithMany(user => user.DeletedExams)
                .HasForeignKey(course => course.DeletedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserEntity>()
                .HasOne(course => course.DeletedBy)
                .WithMany(user => user.DeletedUsers)
                .HasForeignKey(course => course.DeletedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamEntity>()
                .HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .OnDelete(DeleteBehavior.Cascade); // Ensure Restrict

            modelBuilder.Entity<ExamQuestionEntity>()
                .HasMany(q => q.UserAnswers)
                .WithOne(a => a.Question)
                .OnDelete(DeleteBehavior.Cascade); // Ensure Restrict

            modelBuilder.Entity<ExamEntity>()
                .HasMany(e => e.UserExamAttempts)
                .WithOne(uea => uea.Exam)
                .OnDelete(DeleteBehavior.Cascade); // Changed to Restrict

            base.OnModelCreating(modelBuilder);
        }

    }
}