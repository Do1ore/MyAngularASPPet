using System.ComponentModel.DataAnnotations;

namespace MySuperApi.DTOs
{
    public class CreateChatDto
    {
        [Required]
        public string? ChatName { get; set; }
        [Required]
        public List<string>? UserId { get; set; }
    }
}
