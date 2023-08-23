namespace Domain.DTOs;

public class ChatMessageDto
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime SentAt { get; set; }
    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
}