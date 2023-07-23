using Domain.MongoEntities.Chat;

namespace MySuperApi.DTOs.Message;

public class ChatMessageDto
{
    public string? Content { get; set; }
    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }

    public static implicit operator ChatMessage_M(ChatMessageDto dto)
    {
        return new ChatMessage_M()
        {
            Content = dto.Content,
            SenderId = dto.SenderId,
            ChatId = dto.ChatId
        };
    }
}