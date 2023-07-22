using Microsoft.EntityFrameworkCore;
using MySuperApi.Api.DTOs;
using MySuperApi.Domain;
using MySuperApi.Domain.MessageModels;
using MySuperApi.Infrastructure.Repositories.Interfaces;

namespace MySuperApi.Infrastructure.Repositories.Implementation
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
            if (chat == null)
            {
                return new Chat();
            }

            List<ChatMessage> messagesWithSender = await GetMessageWithSenderAsync(chatId);
            var chatUsers = await _db.ChatUsers.Where(a => a.ChatId == chat.Id).ToListAsync();
            chat.Messages = messagesWithSender;
            chat.ChatUsers = chatUsers;
            return chat;
        }

        private async Task<List<ChatMessage>> GetMessageWithSenderAsync(string chatId)
        {
            var messages = from message in _db.Messages
                join sender in _db.Users on message.SenderId equals sender.Id
                where message.ChatId == Guid.Parse(chatId)
                orderby message.SentAt ascending
                select new ChatMessage
                {
                    Id = message.Id,
                    Content = message.Content,
                    SentAt = message.SentAt,
                    SenderId = message.SenderId,
                    Sender = sender,
                    ChatId = message.ChatId
                };
            return await messages.ToListAsync();
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

            await _db.ProfileImageClaims.ExecuteUpdateAsync(p =>
                p.SetProperty(a => a.ProfileImageId, Guid.Parse(imageId)));
        }

        public async Task<ChatMessage> GetMessageDetails(ChatMessage chatMessage)
        {
            var result = await _db.Users.Select(a => new AppUser()
            {
                Email = a.Email,
                Id = a.Id,
                Surname = a.Surname,
                Username = a.Username,
            }).SingleOrDefaultAsync(a => a.Id == chatMessage.SenderId);

            if (result == null)
            {
                return chatMessage;
            }

            chatMessage.Sender = result;
            return chatMessage;
        }

        public async Task<List<AppUser>> SearchUsers(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<AppUser>();
            }

            var users = await _db.Users
                .Where(a => a.Username
                                .Contains(searchTerm)
                            || a.Surname.Contains(searchTerm)
                            || a.Email.Contains(searchTerm))
                .AsNoTracking()
                .ToListAsync();
            var userId = users.Select(i => i.Id).ToList();


            if (users.Count < 0)
            {
                return new List<AppUser>();
            }

            return users;
        }

        public async Task<Chat> CreateChat(CreateChatDto chatDto)
        {
            var appUser = await _db.Users.Where(a => chatDto.UserId.Contains(a.Id.ToString())).ToListAsync();
            List<ChatUser> chatUsers = new();
            var chatId = Guid.NewGuid();

            Chat chat = new Chat()
            {
                Id = chatId,
                Name = chatDto.ChatName,
                ChatAdministrator = Guid.Parse(chatDto.CreatorId),
            };
            foreach (var user in appUser)
            {
                chatUsers.Add(new ChatUser()
                {
                    ChatId = chatId,
                    User = user,
                });
            }

            //Adding admin to chat
            chatUsers.Add(new ChatUser()
            {
                ChatId = chatId,
                UserId = Guid.Parse(chatDto.CreatorId),
            });

            await _db.Chats.AddAsync(chat);
            await _db.ChatUsers.AddRangeAsync(chatUsers);
            await _db.SaveChangesAsync();
            return chat;
        }

        public async Task DeleteChat(string chatId)
        {
            var chatUsers = await _db.ChatUsers
                .Where(a => a.ChatId == Guid.Parse(chatId))
                .Select(a => a.UserId).ToListAsync();

            await _db.ChatUsers.Where(a => a.ChatId == Guid.Parse(chatId)).ExecuteDeleteAsync();
            await _db.Chats.Where(a => a.Id == Guid.Parse(chatId)).ExecuteDeleteAsync();
            await _db.Messages
                .Where(a => a.ChatId == Guid.Parse(chatId))
                .Where(a => chatUsers.Contains(a.SenderId)).ExecuteDeleteAsync();
        }
    }
}