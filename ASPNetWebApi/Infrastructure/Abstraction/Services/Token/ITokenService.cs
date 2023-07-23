using Domain.JWTModels;
using Domain.MongoEntities.User;

namespace Infrastructure.Abstraction.Services.Token;

public interface ITokenService
{
    string CreateToken(AppUserM user);
    RefreshToken CreateRefreshToken();
}