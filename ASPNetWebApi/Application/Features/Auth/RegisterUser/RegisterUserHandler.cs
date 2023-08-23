using Domain.MongoEntities.User;
using Infrastructure.Abstraction.Repositories;
using Infrastructure.Abstraction.Services.Token;
using MediatR;

namespace Application.Features.Auth.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterRequest, AppUserM>
{
    private readonly IPasswordHashService _passwordService;
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(IPasswordHashService passwordService,
        IUserRepository userRepository)
    {
        _passwordService = passwordService;
        _userRepository = userRepository;
    }

    public async Task<AppUserM> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        _passwordService.CreatePasswordHash(request.User.Password, out byte[] passwordHash, out byte[] passwordSalt);
        AppUserM user = new()
        {
            Username = request.User.Username,
            Surname = request.User.Surname,
            Email = request.User.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            AccountCreated = DateTime.UtcNow,
            AccountLastTimeEdited = DateTime.UtcNow,
            LastTimeOnline = DateTime.UtcNow,
        };

        if (await _userRepository.IsUserEmailExists(request.User.Email))
        {
            throw new Exception("User with this email exists");
        }

        await _userRepository.AddUser(user);
        return user;
    }
}