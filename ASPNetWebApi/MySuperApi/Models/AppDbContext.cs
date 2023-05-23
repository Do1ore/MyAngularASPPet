using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MySuperApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserProfileImage> UserProfileImages { get; set; }

        [Obsolete]
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

    }
}
