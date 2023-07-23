using Domain.JWTModels;
using Domain.MongoEntities.User;
using MediatR;

namespace Application.Features.Auth.RegisterUser;

public record RegisterRequest(RegisterUserDto User) : IRequest<AppUserM>;