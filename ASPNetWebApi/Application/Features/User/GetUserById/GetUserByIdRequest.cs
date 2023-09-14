using Domain.DTOs;
using MediatR;

namespace Application.Features.User.GetUserById;

public record GetUserByIdRequest(Guid UserId) : IRequest<AppUserDto>;