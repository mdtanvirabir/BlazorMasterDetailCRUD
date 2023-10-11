using BlazorEvidence.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEvidence.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Modules> Modules { get; set; }
    }
}
