using MySuperApi.Domain.MongoEntities.User;
using MySuperApi.Infrastructure.Repositories.Services.JWTModule;

namespace MySuperApi.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<AppUser_M> AddUser(RegisterUserDto registerUserDto);
}