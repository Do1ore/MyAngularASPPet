using Domain.JWTModels;
using Domain.MongoEntities.Chat;
using Domain.MongoEntities.User;

namespace Infrastructure.Abstraction.Repositories;

public interface IUserRepository
{
    Task<AppUser_M> AddUser(AppUser_M user);
    Task<List<Chat_M>> GetChatsForUser(string userId);

    Task<bool> IsUserExists(Guid userId);
    Task<bool> IsUserEmailExists(string email);
    
    Task<AppUser_M> GetUserById(Guid userId);
    Task<AppUser_M> GetUserByEmail(string email);
    Task<string> UpdateUserRefreshToken(Guid userId, RefreshToken newRefreshToken);
}