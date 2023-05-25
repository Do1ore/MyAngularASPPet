using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models.MessageModels
{
    public class ChatUser
    {
        [ForeignKey("AppUser")]
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }
        [ForeignKey("Chat")]
        public Guid ChatId { get; set; }
        public Chat? Chat { get; set; }
    }

}
