using Infrastructure.Abstraction.Repositories;
using Infrastructure.Abstraction.Services.Token;
using Infrastructure.Abstraction.Services.User;
using MediatR;

namespace Application.Features.Auth.SignIn;

public class SignInHandler : IRequestHandler<SingInRequest, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpUserDataAccessorService _userDataAccessor;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHashService _passwordHashService;

    public SignInHandler(IUserRepository userRepository,
        IHttpUserDataAccessorService userDataAccessor,
        ITokenService tokenService,
        IPasswordHashService passwordHashService)
    {
        _userRepository = userRepository;
        _userDataAccessor = userDataAccessor;
        _tokenService = tokenService;
        _passwordHashService = passwordHashService;
    }

    public async Task<string> Handle(SingInRequest request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsUserEmailExists(request.UserDto.Email!))
        {
            throw new ArgumentException("User with this email not found");
        }

        var user = await _userRepository.GetUserByEmail(request.UserDto.Email!);

        if (!_passwordHashService.VerifyPasswordHash(request.UserDto.Password!, user.PasswordHash!, user.PasswordSalt!))
        {
            throw new ArgumentException("Invalid password");
        }

        var token = _tokenService.CreateToken(user);

        var refreshToken = _tokenService.CreateRefreshToken();
        await _userRepository.UpdateUserRefreshToken(user.Id, refreshToken);
        _userDataAccessor.AppendRefreshToken(refreshToken, request.Response);


        return token;
    }
}