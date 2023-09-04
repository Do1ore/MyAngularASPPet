using System.ComponentModel.DataAnnotations;
using Domain.MongoEntities.Chat;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs
{
    public class CreateChatDto
    {
        [Required] public string? ChatName { get; init; }
        [Required] public List<Guid>? UserIds { get; init; }
        [Required] public Guid CreatorId { get; init; }
        public IFormFile? ChatImage { get; init; }
        
        public static implicit operator ChatM(CreateChatDto dto)
        {
            return new ChatM()
            {
                Name = dto.ChatName,
                AppUserIds = dto.UserIds!,
                ChatAdministrator = dto.CreatorId,
                ChatImageFile = dto.ChatImage,
            };
        }
    }
}