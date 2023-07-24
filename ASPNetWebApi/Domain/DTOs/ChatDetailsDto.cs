using Domain.MongoEntities.Chat;

namespace Domain.DTOs;

/// <summary>
/// All information that user can see in chat
/// </summary>
public class ChatDetailsDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? LastMessage { get; set; }

    public Guid? ChatAdministrator { get; set; }

    public List<Guid>? UserIds { get; set; } = new List<Guid>();
    public ICollection<ChatMessageM> Messages { get; set; } = new List<ChatMessageM>();
    public ICollection<AppUserDto> AppUsers { get; set; } = new List<AppUserDto>();
}