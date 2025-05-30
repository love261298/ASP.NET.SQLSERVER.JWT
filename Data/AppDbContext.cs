using ASP.NET.SQLSERVER.JWT.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
