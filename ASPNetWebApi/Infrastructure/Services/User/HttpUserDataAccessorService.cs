using Domain.JWTModels;
using Infrastructure.Abstraction.Services.User;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.User
{
    public class HttpUserDataAccessorService : IHttpUserDataAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUserDataAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetMyId()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                //result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return result;
        }

        public void AppendRefreshToken(RefreshToken newRefreshToken, HttpResponse response)
        {
            CookieOptions cookieOptions = new()
            {
                Secure = true,
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                SameSite = SameSiteMode.None,
            };
            response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            response.ContentType = "application/json";
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                //result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }
    }
}