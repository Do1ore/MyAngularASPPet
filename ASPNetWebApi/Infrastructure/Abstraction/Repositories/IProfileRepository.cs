namespace Infrastructure.Abstraction.Repositories;

public interface IProfileRepository
{
    Task UpdateCurrentProfileImage(string imageId, string userId);
    Task<string> GetProfileImage(string userId);
}