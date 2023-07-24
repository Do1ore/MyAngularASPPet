using Domain.DTOs;
using Domain.MongoEntities.User;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public partial class UserMapper
{
   public partial AppUserDto AppUserToAppUserDto(AppUserM user);
}