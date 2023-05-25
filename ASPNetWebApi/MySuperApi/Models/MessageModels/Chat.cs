using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models.MessageModels
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [NotMapped]
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        [NotMapped]
        public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
    }

}
