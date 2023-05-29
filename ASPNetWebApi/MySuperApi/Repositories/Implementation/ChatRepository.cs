using Microsoft.EntityFrameworkCore;
using MySuperApi.Models;
using MySuperApi.Models.MessageModels;
using MySuperApi.Repositories.Interfaces;
using NuGet.Protocol.Plugins;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace MySuperApi.Repositories.Implementation
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ChatRepository> _logger;
        public ChatRepository(AppDbContext db, ILogger<ChatRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Chat>> GetChatsForUser(string userId)
        {
            var userChats = await _db.ChatUsers
                .Where(i => i.UserId == Guid.Parse(userId))
                .Select(i => i.ChatId)
                .ToListAsync();


            return await _db.Chats.Where(a => userChats.Contains(a.Id)).ToListAsync();
        }

        public async Task<Chat> GetChatDetails(string userId, string chatId)
        {
            var chat = await _db.Chats.AsNoTracking().SingleOrDefaultAsync(a => a.Id == Guid.Parse(chatId));
            if (chat == null) { return new Chat(); }

            //.AsNoTracking().Where(a => a.ChatId == chat.Id).ToListAsync();
            var messagesWithSender = from message in _db.Messages
                                     join sender in _db.Users on message.SenderId equals sender.Id
                                     select new ChatMessage
                                     {
                                         Id = message.Id,
                                         Content = message.Content,
                                         SentAt = message.SentAt,
                                         SenderId = message.SenderId,
                                         Sender = sender,
                                         ChatId = message.ChatId
                                     };
            var chatUsers = await _db.ChatUsers.Where(a=>a.ChatId == chat.Id).ToListAsync();
            chat.Messages= await messagesWithSender.ToListAsync();
            chat.ChatUsers = chatUsers;
            return chat;
        }

        public async Task<string> GetProfileImage(string userId)
        {
            var imageId = await _db.ProfileImageClaims
                .Where(a => a.UserId == Guid.Parse(userId))
                .Select(a => a.ProfileImageId)
                .SingleAsync();

            var image = await _db.ProfileImages.SingleOrDefaultAsync(i => i.ImageId == imageId);
            if (image == null)
            {
                return "";
            }
            if (string.IsNullOrEmpty(image.ImagePath))
            {
                return "";

            }
            return image.ImagePath;
        }

        public async Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent)
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
            return chatMessage;
        }

        public async Task UpdateCurrentProfileImage(string imageId, string userId)
        {
            if (!await _db.ProfileImageClaims.AsNoTracking().AnyAsync(a => a.UserId == Guid.Parse(userId)))
            {
                _logger.LogInformation("Not image claims found.");

                await _db.AddAsync(new ProfileImageClaims()
                {
                    ProfileImageId = Guid.Parse(imageId),
                    UserId = Guid.Parse(userId),
                });
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Image claims created with userId <{userId}> and imageId <{imageId}>");
                return;
            }

            await _db.ProfileImageClaims.ExecuteUpdateAsync(p => p.SetProperty(a => a.ProfileImageId, Guid.Parse(imageId)));
        }
    }
}
