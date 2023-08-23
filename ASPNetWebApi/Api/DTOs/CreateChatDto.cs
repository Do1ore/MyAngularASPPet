using Domain.MongoEntities.Chat;
using Microsoft.Build.Framework;

namespace MySuperApi.DTOs
{
    public class CreateChatDto
    {
        [Required] public string? ChatName { get; set; }
        [Required] public List<Guid>? UserIds { get; set; }
        [Required] public Guid CreatorId { get; set; }


        public static implicit operator ChatM(CreateChatDto dto)
        {
            return new ChatM()
            {
                Name = dto.ChatName,
                AppUserIds = dto.UserIds!,
                ChatAdministrator = dto.CreatorId
            };
        }
    }
}