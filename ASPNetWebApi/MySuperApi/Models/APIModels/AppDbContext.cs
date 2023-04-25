using Microsoft.EntityFrameworkCore;

namespace MySuperApi.Models.APIModels
{
    public class AppDbContext : DbContext
    {
        public DbSet<MyUser> Users { get; set; }
        public DbSet<ApiImageModel> ApiImages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
