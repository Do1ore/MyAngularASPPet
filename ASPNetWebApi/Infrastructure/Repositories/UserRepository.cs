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
    private readonly IMongoCollection<ChatM> _chatCollection;

    public UserRepository(IMongoDatabase database)
    {
        _chatCollection = database.GetCollection<ChatM>(MongoCollectionName.Chat);
        _userCollection = database.GetCollection<AppUserM>(MongoCollectionName.User);
    }

    public async Task<AppUserM> AddUser(AppUserM user)
    {
        await _userCollection.InsertOneAsync(user);

        return user;
    }

    public async Task<List<ChatM>> GetChatsForUser(Guid userId)
    {
        var user = await _userCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
        if (user is null)
        {
            throw new ArgumentException("User with this id is not found");
        }

        var chatFilter = Builders<ChatM>.Filter.AnyEq(chat => chat.AppUserIds, userId);
        var chats = await _chatCollection.Find(chatFilter).ToListAsync();


        if (chats.Count < 1 || chats is null)
        {
            throw new ArgumentException($"No chats found for user with id: {userId}");
        }

        return chats;
    }

    public async IAsyncEnumerable<AppUserM> GetUsersDetails(List<Guid> userIds)
    {
        foreach (var userid in userIds)
        {
            yield return await _userCollection
                .Find(Builders<AppUserM>.Filter
                    .Eq(u => u.Id, userid))
                .FirstOrDefaultAsync();
        }
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

    public async Task<List<AppUserM>> FindUsers(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        var userFilter = Builders<AppUserM>.Filter
            .Where(u => u.Email.ToLower().Contains(searchTerm) ||
                        u.Username.ToLower().Contains(searchTerm) ||
                        u.Surname.ToLower().Contains(searchTerm));


        var options = new FindOptions<AppUserM>
        {
            Limit = 10
        };

        var users = await _userCollection.FindAsync(userFilter, options);
        return users.ToList();
    }
}