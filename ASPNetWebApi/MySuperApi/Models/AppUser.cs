using MySuperApi.Models.MessageModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models
{
    public class AppUser
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime AccountLastTimeEdited { get; set; }
        public DateTime LastTimeOnline { get; set; }
        public byte[]? CurrentImageBytes { get; set; }
        public ICollection<UserProfileImage> UserProfileImages { get; set; } = new List<UserProfileImage>();
        [NotMapped]
        public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
        [NotMapped]
        public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();

    }
}
