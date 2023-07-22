using MongoDB.Driver;
using MySuperApi.Domain.MongoEntities.User;
using MySuperApi.Infrastructure.Repositories.Interfaces;
using MySuperApi.Infrastructure.Repositories.Services.JWTModule;

namespace MySuperApi.Infrastructure.Repositories.Implementation.Mongo;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUser_M> _chatUsers;

    public UserRepository(IMongoDatabase database)
    {
        _chatUsers = database.GetCollection<AppUser_M>("UsersCollection");
    }

    public async Task<AppUser_M> AddUser(RegisterUserDto registerUserDto)
    {
        AppUser_M user = new()
        {
            Username = registerUserDto.Username,
            Surname = registerUserDto.Surname,
            Email = registerUserDto.Email,
            PasswordHash = new[] { byte.MinValue },
            PasswordSalt = new[] { byte.MinValue },
            AccountCreated = DateTime.UtcNow,
            AccountLastTimeEdited = DateTime.UtcNow,
            LastTimeOnline = DateTime.UtcNow,
        };
        await _chatUsers.InsertOneAsync(user);
        return user;
    }
}