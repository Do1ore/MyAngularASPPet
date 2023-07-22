using MongoDB.Driver;
using MySuperApi.Api.DTOs;
using MySuperApi.Domain.MongoEntities.Chat;
using MySuperApi.Infrastructure.Repositories.Interfaces;
using MySuperApi.Domain.MongoEntities.User;

namespace MySuperApi.Infrastructure.Repositories.Implementation.Mongo;

public class ChatRepository : IMongoChatRepository
{
    private readonly IMongoCollection<Chat_M> _chatCollection;
    private readonly IMongoCollection<AppUser_M> _chatUsers;

    public ChatRepository(IMongoDatabase database)
    {
        _chatCollection = database.GetCollection<Chat_M>("ChatCollection");
        _chatUsers = database.GetCollection<AppUser_M>("UsersCollection");
    }

    public async Task<Chat_M> CreateChat(CreateChatDto chatDto)
    {
        var users = new List<AppUser_M>();
        foreach (var id in chatDto.UserIds)
        {
            var user = await _chatUsers.FindAsync(a => a.Id == Guid.Parse(id));
            users.Add(user.First());
        }

        var admin = await _chatUsers.FindAsync(a => a.Id == Guid.Parse(chatDto.CreatorId));
        var chat = new Chat_M()
        {
            Name = chatDto.ChatName,
            ChatUsers = users,
            ChatAdministrator = Guid.Parse(chatDto.CreatorId),
        };
        await _chatCollection.InsertOneAsync(chat);
        
        return chat;
    }

    public Task<List<Chat_M>> GetChatsForUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Chat_M> UpdateChat(CreateChatDto chatDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteChat(string chatId)
    {
        throw new NotImplementedException();
    }

    public Task<Chat_M> GetChatDetails(string userId, string chatId)
    {
        throw new NotImplementedException();
    }
}