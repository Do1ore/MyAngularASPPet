using Domain.JWTModels;
using Domain.MongoEntities.Chat;
using Domain.MongoEntities.User;

namespace Infrastructure.Abstraction.Repositories;

public interface IUserRepository
{
    Task<AppUserM> AddUser(AppUserM user);
    Task<List<ChatM>> GetChatsForUser(string userId);

    Task<bool> IsUserExists(Guid userId);
    Task<bool> IsUserEmailExists(string email);
    
    Task<AppUserM> GetUserById(Guid userId);
    Task<AppUserM> GetUserByEmail(string email);
    Task<string> UpdateUserRefreshToken(Guid userId, RefreshToken newRefreshToken);
}