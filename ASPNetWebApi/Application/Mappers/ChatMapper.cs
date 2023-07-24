using Domain.DTOs;
using Domain.MongoEntities.Chat;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public partial class ChatMapper
{
    [MapperIgnoreTarget(nameof(ChatDetailsDto.AppUsers))]
    [MapperIgnoreTarget(nameof(ChatM.AppUserIds))]
    public partial ChatDetailsDto ChatToChatDto(ChatM chat);
}