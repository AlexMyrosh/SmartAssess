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

        public DbSet<QuestionOptionEntity> QuestionOptions { get; set; }
    }
}