using Microsoft.EntityFrameworkCore;
using MySuperApi.Models;
using MySuperApi.Models.MessageModels;
using MySuperApi.Repositories.Interfaces;

namespace MySuperApi.Repositories.Implementation
{
    public class ChatResository : IChatRepository
    {
        private readonly AppDbContext _db;

        public ChatResository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Chat>> GetAllChatsForUser(string userId)
        {
            var userChats = await _db.ChatUsers
                .Where(i => i.UserId == Guid.Parse(userId))
                .Select(i => i.ChatId)
                .ToListAsync();


            return await _db.Chats
                    .Join(
                        _db.ChatUsers,
                        chat => chat.Id,
                        chatUser => chatUser.ChatId,
                        (chat, chatUser) => new { Chat = chat, ChatUser = chatUser }
                    )
                    .Join(
                        _db.Users,
                        chatUser => chatUser.ChatUser.UserId,
                        user => user.Id,
                        (chatUser, user) => new { Chat = chatUser.Chat, User = user }
                    )
                    .GroupBy(result => result.Chat)
                    .Select(group => new Chat
                    {
                        Id = group.Key.Id,
                        Name = group.Key.Name,
                        Messages = _db.Messages
                            .Where(message => message.ChatId == group.Key.Id)
                            .ToList(),
                        ChatUsers = group.Select(result => new ChatUser
                        {
                            ChatId = result.Chat.Id,
                            UserId = result.User.Id,
                            User = result.User
                        }).ToList()
                    })
                    .ToListAsync();
        }

        public async Task SendMessage(string chatId, string senderId, string messageContent)
        {
            ChatMessage chatMessage = new()
            {
                ChatId = Guid.Parse(chatId),
                SenderId = Guid.Parse(senderId),
                Content = messageContent,
                SentAt = DateTime.UtcNow,
            };
            await _db.Chats
                .Where(i => i.Id == Guid.Parse(chatId))
                .ExecuteUpdateAsync(p => p
                .SetProperty(a => a.Lastmessage, a => messageContent));
            await _db.Messages.AddAsync(chatMessage);
            await _db.SaveChangesAsync();
        }
    }
}
