using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Context
{
    public class SqlContext : IdentityDbContext<IdentityUser>
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }
    }
}