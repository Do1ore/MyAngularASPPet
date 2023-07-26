using Application.Mappers;
using Domain.DTOs;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.User.SearchUser;

public class SearchUserRequestHandler : IRequestHandler<SearchUserRequest, List<AppUserDto>>
{
    private readonly IUserRepository _userRepository;

    public SearchUserRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<AppUserDto>> Handle(SearchUserRequest request, CancellationToken cancellationToken)
    {
        var appUsers = await _userRepository.FindUsers(request.SearchTerm);
        var mapper = new UserMapper();

        var dtoUsers = new List<AppUserDto>();
        foreach (var user in appUsers)
        {
            dtoUsers.Add(mapper.AppUserToAppUserDto(user));
        }

        return dtoUsers;
    }
}