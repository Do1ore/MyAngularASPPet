using Microsoft.EntityFrameworkCore;
using MySuperApi.Models.MessageModels;

namespace MySuperApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<ProfileImageClaims> ProfileImageClaims { get; set; }
        public DbSet<UserProfileImage> ProfileImages { get; set; }
        public DbSet<UserProfileImageStorage> ProfileImageStorages { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }


        [Obsolete]
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileImageClaims>()
                .HasKey(k => new { k.UserId, k.ProfileImageId });

            modelBuilder.Entity<ChatUser>()
                .HasKey(k => new { k.UserId, k.ChatId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
