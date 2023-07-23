using System.ComponentModel.DataAnnotations;
using Domain.MongoEntities.Chat;

namespace MySuperApi.DTOs
{
    public class CreateChatDto
    {
        [Required] public string? ChatName { get; set; }
        [Required] public List<Guid>? UserIds { get; set; }
        [Required] public Guid? CreatorId { get; set; }


        public static implicit operator ChatM(CreateChatDto dto)
        {
            return new ChatM()
            {
                Name = dto.ChatName,
                UserIds = dto.UserIds,
                ChatAdministrator = dto.CreatorId
            };
        }
    }
}