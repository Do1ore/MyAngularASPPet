using System.ComponentModel.DataAnnotations;
using MySuperApi.Domain.MongoEntities.Chat;

namespace MySuperApi.Api.DTOs
{
    public class CreateChatDto
    {
        [Required]
        public string? ChatName { get; set; }
        [Required]
        public List<string>? UserIds { get; set; }
        [Required]
        public string? CreatorId { get; set; }
        
    }
}
