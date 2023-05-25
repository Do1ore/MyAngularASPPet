using MySuperApi.Models;

namespace MySuperApi.Services.ProfileImageService
{
    public class ProfileImageService : IProfileImageService
    {
        public UserProfileImage CreateProfileImageModel(string filename, string webpath, string userId)
        {
            UserProfileImage profileImage = new UserProfileImage()
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
