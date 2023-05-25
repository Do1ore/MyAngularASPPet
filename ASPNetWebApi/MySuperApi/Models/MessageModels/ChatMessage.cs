using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models.MessageModels
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }

        [ForeignKey("AppUser")]
        public Guid SenderId { get; set; }
        [NotMapped]
        public AppUser? Sender { get; set; }
        
        [ForeignKey("Chat")]
        public Guid ChatId { get; set; }
        [NotMapped]
        public Chat? Chat { get; set; }
    }

}
