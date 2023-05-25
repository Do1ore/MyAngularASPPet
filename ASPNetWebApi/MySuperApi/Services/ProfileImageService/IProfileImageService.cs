using MySuperApi.Models;

namespace MySuperApi.Services.ProfileImageService
{
    public interface IProfileImageService
    {
        UserProfileImage CreateProfileImageModel(string filename, string webpath, string userId);
    }

}
