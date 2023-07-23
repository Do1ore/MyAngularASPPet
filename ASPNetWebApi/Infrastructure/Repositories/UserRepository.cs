using Domain.Constants;
using Domain.JWTModels;
using Domain.MongoEntities.Chat;
using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUser_M> _userCollection;

    public UserRepository(IMongoDatabase database)
    {
        _userCollection = database.GetCollection<AppUser_M>(MongoCollectionName.User);
    }

    public async Task<AppUser_M> AddUser(AppUser_M user)
    {
        await _userCollection.InsertOneAsync(user);

        return user;
    }

    public Task<List<Chat_M>> GetChatsForUser(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserExists(Guid userId)
    {
        var filter = Builders<AppUser_M>.Filter.Eq(u => u.Id, userId);
        var count = await _userCollection.CountDocumentsAsync(filter);

        return count > 0;
    }

    public async Task<bool> IsUserEmailExists(string email)
    {
        var filter = Builders<AppUser_M>.Filter.Eq(u => u.Email, email);
        var count = await _userCollection.CountDocumentsAsync(filter);

        return count > 0;
    }

    public async Task<AppUser_M> GetUserById(Guid userId)
    {
        var filter = Builders<AppUser_M>.Filter.Eq(u => u.Id, userId);
        return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<AppUser_M> GetUserByEmail(string email)
    {
        var filter = Builders<AppUser_M>.Filter.Eq(u => u.Email, email);
        return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<string> UpdateUserRefreshToken(Guid userId, RefreshToken newRefreshToken)
    {
        var filter = Builders<AppUser_M>.Filter.Eq(a => a.Id, userId);
        var user = await _userCollection.Find(filter).SingleOrDefaultAsync() ??
                   throw new ArgumentException("User not found");

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;

        var update = Builders<AppUser_M>.Update
            .Set(u => u.RefreshToken, newRefreshToken.Token)
            .Set(u => u.TokenCreated, newRefreshToken.Created)
            .Set(u => u.TokenExpires, newRefreshToken.Expires);


        var result = await _userCollection.FindOneAndUpdateAsync(filter, update) ??
                     throw new ArgumentException("Cannot update user");

        return result.RefreshToken;
    }
}