using Domain.MongoEntities.User;

namespace Infrastructure.Abstraction.Services.User
{
    public interface IProfileImageService
    {
        UserProfileImageM CreateProfileImageModel(string filename, string webpath, string userId);
    }

}
