using MySuperApi.Domain;

namespace MySuperApi.Infrastructure.Repositories.Services.ProfileImageService
{
    public interface IProfileImageService
    {
        UserProfileImage CreateProfileImageModel(string filename, string webpath, string userId);
    }

}
