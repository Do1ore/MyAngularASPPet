using System.ComponentModel.DataAnnotations;

namespace MySuperApi.Models
{
    public class AppUser
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime AccountLastTimeEdited { get; set; }    
        public DateTime LastTimeOnline { get; set; }
        public Guid CurrentImage { get; set; }
        public ICollection<UserProfileImage> UserProfileImages { get; set; } = new List<UserProfileImage>();

    }
}
