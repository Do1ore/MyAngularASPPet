using Domain.DTOs;
using Domain.MongoEntities.Chat;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public partial class MessageMapper
{
    public partial ChatMessageDto MessageToMessageDto(ChatMessageM chat);
}