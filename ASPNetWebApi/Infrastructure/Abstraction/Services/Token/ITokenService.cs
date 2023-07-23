using Domain.JWTModels;
using Domain.MongoEntities.User;

namespace Infrastructure.Abstraction.Services.Token;

public interface ITokenService
{
    string CreateToken(AppUser_M user);
    RefreshToken CreateRefreshToken();
}