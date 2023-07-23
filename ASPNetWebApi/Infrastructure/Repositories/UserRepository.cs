using Domain.Constants;
using Domain.JWTModels;
using Domain.MongoEntities.Chat;
using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUserM> _userCollection;

    public UserRepository(IMongoDatabase database)
    {
        _userCollection = database.GetCollection<AppUserM>(MongoCollectionName.User);
    }

    public async Task<AppUserM> AddUser(AppUserM user)
    {
        await _userCollection.InsertOneAsync(user);

        return user;
    }

    public Task<List<ChatM>> GetChatsForUser(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserExists(Guid userId)
    {
        var filter = Builders<AppUserM>.Filter.Eq(u => u.Id, userId);
        var count = await _userCollection.CountDocumentsAsync(filter);

        return count > 0;
    }

    public async Task<bool> IsUserEmailExists(string email)
    {
        var filter = Builders<AppUserM>.Filter.Eq(u => u.Email, email);
        var count = await _userCollection.CountDocumentsAsync(filter);

        return count > 0;
    }

    public async Task<AppUserM> GetUserById(Guid userId)
    {
        var filter = Builders<AppUserM>.Filter.Eq(u => u.Id, userId);
        return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<AppUserM> GetUserByEmail(string email)
    {
        var filter = Builders<AppUserM>.Filter.Eq(u => u.Email, email);
        return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<string> UpdateUserRefreshToken(Guid userId, RefreshToken newRefreshToken)
    {
        var filter = Builders<AppUserM>.Filter.Eq(a => a.Id, userId);
        var user = await _userCollection.Find(filter).SingleOrDefaultAsync() ??
                   throw new ArgumentException("User not found");

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;

        var update = Builders<AppUserM>.Update
            .Set(u => u.RefreshToken, newRefreshToken.Token)
            .Set(u => u.TokenCreated, newRefreshToken.Created)
            .Set(u => u.TokenExpires, newRefreshToken.Expires);


        var result = await _userCollection.FindOneAndUpdateAsync(filter, update) ??
                     throw new ArgumentException("Cannot update user");

        return result.RefreshToken;
    }
}