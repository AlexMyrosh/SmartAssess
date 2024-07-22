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

        public DbSet<TeacherNoteEntity> TeacherNotes { get; set; }

        public DbSet<UserAnswerEntity> UserAnswers { get; set; }

        public DbSet<UserExamPassEntity> UserExamPasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExamQuestionEntity>()
                .HasOne(eq => eq.TeacherNote)
                .WithOne(tn => tn.Question)
                .HasForeignKey<TeacherNoteEntity>(tn => tn.QuestionId);

            modelBuilder.Entity<UserAnswerEntity>()
                .HasOne<UserExamPassEntity>(s => s.StudentExamPass)
                .WithMany(g => g.UserAnswers)
                .HasForeignKey(s => s.StudentExamPassId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}