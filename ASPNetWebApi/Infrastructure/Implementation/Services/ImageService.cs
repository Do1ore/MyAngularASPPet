using Domain.Constants;
using Domain.Enums;
using Domain.MongoEntities.Chat;
using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Implementation.Services;

public class ImageService : IImageService
{
    private readonly string _compressedImageFolderPath;
    private readonly string _folderForUsers;
    private readonly string _folderForChats;
    private readonly IMongoCollection<AppUserM> _userCollection;
    private readonly IMongoCollection<ChatM> _chatCollection;
    private readonly ILogger<ImageService> _logger;

    public ImageService(IConfiguration configuration,
        IMongoDatabase mongoDatabase, ILogger<ImageService> logger)
    {
        _logger = logger;

        _compressedImageFolderPath = configuration.GetSection("Images:CompressedImages").Value ??
                                     throw new ArgumentException("Path for image folder not found");

        _folderForUsers = configuration.GetSection("Images:FolderForUsers").Value ??
                          throw new ArgumentException("FolderForUsers not found");

        _folderForChats = configuration.GetSection("Images:FolderForChats").Value ??
                          throw new ArgumentException("FolderForChats not found");

        _userCollection = mongoDatabase.GetCollection<AppUserM>(MongoCollectionName.User);
        _chatCollection = mongoDatabase.GetCollection<ChatM>(MongoCollectionName.Chat);
    }

    public async Task<string> UploadChatProfileImage(IFormFile image, Guid chatId)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid image file");
        }

        var filePath = CreateFilePath(image, chatId, FileFor.ChatProfile);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        var filter = Builders<ChatM>.Filter.Eq(c => c.Id, chatId);

        var update = Builders<ChatM>.Update.Set(a => a.ImagePath, filePath);

        var chat = await _chatCollection.FindOneAndUpdateAsync(filter, update);
        _logger.LogInformation("Chat after new profile image: {@Chat}", chat);
        return filePath;
    }

    public async Task<string> GetChatProfileImage(Guid chatId)
    {
        var filter = Builders<ChatM>.Filter.Eq(c => c.Id, chatId);
        var chat = await _chatCollection.Find(filter).FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(chat.ImagePath))
        {
            throw new ArgumentException("Chat have no image path");
        }

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), chat.ImagePath);
        return fullPath;
    }


    public async Task<string> UploadUserProfileImage(IFormFile image, Guid userId)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid image file");
        }

        var filePath = CreateFilePath(image, userId, FileFor.UserProfile);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        var filter = Builders<AppUserM>.Filter.Eq(c => c.Id, userId);

        var update = Builders<AppUserM>.Update.Set(a => a.ProfileImagePath, filePath);

        var user = await _userCollection.FindOneAndUpdateAsync(filter, update);
        _logger.LogInformation("User after new profile image: {@User}", user);
        return filePath;
    }

    public async Task<string> GetUserProfileImage(Guid userId)
    {
        var filter = Builders<AppUserM>.Filter.Eq(c => c.Id, userId);
        var appUser = await _userCollection.Find(filter).FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(appUser.ProfileImagePath))
        {
            throw new ArgumentException("User have no image path");
        }

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), appUser.ProfileImagePath);
        return fullPath;
    }

    private string CreateFilePath(IFormFile formFile, Guid guid, FileFor fileFor)
    {
        var uniqueFileName = "";
        var filePath = "";
        switch (fileFor)
        {
            case FileFor.ChatProfile:

                uniqueFileName = guid + "_" + DateTime.Now.ToString("s").Replace(":", "-") + "_" +
                                 formFile.FileName;
                filePath = Path.Combine(_compressedImageFolderPath, _folderForChats, uniqueFileName);
                return filePath;

            case FileFor.UserProfile:
                uniqueFileName = guid + "_" + DateTime.Now.ToString("s").Replace(":", "-") + "_" +
                                 formFile.FileName;
                filePath = Path.Combine(_compressedImageFolderPath, _folderForUsers, uniqueFileName);
                return filePath;

            default:
                throw new ArgumentException("Invalid argument");
        }
    }
}