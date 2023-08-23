using Infrastructure.Abstraction.Repositories;
using Infrastructure.Abstraction.Services.Token;
using Infrastructure.Abstraction.Services.User;
using MediatR;

namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, string>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IHttpUserDataAccessorService _userDataAccessor;

    public RefreshTokenRequestHandler(IUserRepository userRepository,
        ITokenService tokenService, IHttpUserDataAccessorService userDataAccessor)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _userDataAccessor = userDataAccessor;
    }

    public async Task<string> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.UserId);

        string? refreshToken = request.HttpRequest.Cookies["refreshToken"];

        if (!user.RefreshToken.Equals(refreshToken))
        {
            throw new ApplicationException("Invalid Refresh Token.");
        }

        if (user.TokenExpires < DateTime.UtcNow)
        {
            throw new ApplicationException("Token expired.");
        }

        string token = _tokenService.CreateToken(user);
        Domain.JWTModels.RefreshToken newRefreshToken = _tokenService.CreateRefreshToken();

        _userDataAccessor.AppendRefreshToken(newRefreshToken, request.HttpResponse);
        await _userRepository.UpdateUserRefreshToken(user.Id, newRefreshToken);
        return token;
    }
}