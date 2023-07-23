using Domain.MongoEntities.User;

namespace Infrastructure.Abstraction.Services.User
{
    public interface IProfileImageService
    {
        UserProfileImage_M CreateProfileImageModel(string filename, string webpath, string userId);
    }

}
