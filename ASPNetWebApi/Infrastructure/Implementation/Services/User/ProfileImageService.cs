using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Services.User;

namespace Infrastructure.Implementation.Services.ProfileImageService
{
    public class ProfileImageService : IProfileImageService
    {
        public UserProfileImageM CreateProfileImageModel(string filename, string webpath, string userId)
        {
            UserProfileImageM profileImage = new UserProfileImageM()
            {
                AppUserId = Guid.Parse(userId),
                FileName = filename,
                ImagePath = webpath,
                ImageId = Guid.NewGuid(),
            };
            return profileImage;
        }

    }
}
