// using Domain;
// using Domain.MessageModels;
// using MySuperApi.Api.DTOs;
//
// namespace Infrastructure.Abstraction
// {
//     public interface ILegacyChatRepository
//     {
//         Task<Chat> CreateChat(CreateChatDto chatDto);
//         Task<string> GetProfileImage(string userId);
//         Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent);
//         Task<List<Chat>> GetChatsForUser(string userId);
//         Task<Chat> GetChatDetails(string userId, string chatId);
//         Task<ChatMessage> GetMessageDetails(ChatMessage message);
//         Task UpdateCurrentProfileImage(string imageId, string userId);
//         Task<List<AppUser>> SearchUsers(string searchTerm);
//         Task DeleteChat(string chatId);
//     }
// }