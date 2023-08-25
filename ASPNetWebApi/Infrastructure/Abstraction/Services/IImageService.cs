using Microsoft.AspNetCore.Http;

namespace Infrastructure.Abstraction.Services;

public interface IImageService
{
    Task<string> UploadChatProfileImage(IFormFile image, Guid chatId);
    Task<string> GetChatProfileImage(Guid chatId);
    Task<string> UploadUserProfileImage(IFormFile image, Guid userId);
    Task<string> GetUserProfileImage(Guid userId);
}