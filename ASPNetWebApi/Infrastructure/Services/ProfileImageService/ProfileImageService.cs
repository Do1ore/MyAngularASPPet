using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Services.User;

namespace Infrastructure.Services.ProfileImageService
{
    public class ProfileImageService : IProfileImageService
    {
        public UserProfileImage_M CreateProfileImageModel(string filename, string webpath, string userId)
        {
            UserProfileImage_M profileImage = new UserProfileImage_M()
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
