using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Context
{
    public class SqlContext : IdentityDbContext<User>
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }
    }
}