using Amazon.Util.Internal;
using Domain.Constants;
using Domain.MongoEntities.Chat;
using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly string _compressedImageFolderPath;
    private readonly IMongoCollection<AppUserM> _userCollection;
    private readonly IMongoCollection<ChatM> _chatCollection;

    public ImageService(IConfiguration configuration,
        IMongoDatabase mongoDatabase)
    {
        _compressedImageFolderPath = configuration.GetSection("Images:CompressedImages").Value ??
                                     throw new ArgumentException("Path for image folder not found");
        _userCollection = mongoDatabase.GetCollection<AppUserM>(MongoCollectionName.User);
        _chatCollection = mongoDatabase.GetCollection<ChatM>(MongoCollectionName.Chat);
    }

    public async Task<string> UploadChatProfileImage(IFormFile image, Guid chatId)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid image file");
        }

        var uniqueFileName = chatId + "_" + DateTime.Now.ToString("s").Replace(":", "-") + "_" +
                             image.FileName;

        var filePath = Path.Combine(_compressedImageFolderPath, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        var filter = Builders<ChatM>.Filter.Eq(c => c.Id, chatId);

        var update = Builders<ChatM>.Update.Set(a => a.ImagePath, filePath);

        var chat = await _chatCollection.FindOneAndUpdateAsync(filter, update);

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


    public Task<string> UploadUserProfileImage(IFormFile image, Guid chatId)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetChatUserProfileImage(Guid chatId)
    {
        throw new NotImplementedException();
    }
}