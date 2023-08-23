using Domain.DTOs;
using MediatR;

namespace Application.Features.User.SearchUser;

public record SearchUserRequest(string SearchTerm): IRequest<List<AppUserDto>>;