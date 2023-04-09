using JWTModule;
using Microsoft.EntityFrameworkCore;

namespace MySuperApi.JWTModule.Models
{
    public class MyUserDbContext : DbContext
    {
        public DbSet<MyUser> Users { get; set; }

        public MyUserDbContext(DbContextOptions<MyUserDbContext> options) : base(options) { }

    }
}
