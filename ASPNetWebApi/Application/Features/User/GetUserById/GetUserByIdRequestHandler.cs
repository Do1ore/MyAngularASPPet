using Application.Mappers;
using Domain.DTOs;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.User.GetUserById;

public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, AppUserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AppUserDto> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        var mapper = new UserMapper();
        
        return mapper.AppUserToAppUserDto(user);
    }
}