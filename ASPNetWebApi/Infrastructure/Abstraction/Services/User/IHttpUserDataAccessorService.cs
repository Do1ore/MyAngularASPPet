using Domain.JWTModels;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Abstraction.Services.User
{
    public interface IHttpUserDataAccessorService
    {
        string GetMyName();
        string GetMyId();
        
        void AppendRefreshToken(RefreshToken newRefreshToken, HttpResponse response);
    }
}
