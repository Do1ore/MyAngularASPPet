using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.SignIn;

public record SingInRequest(UserDto UserDto, HttpResponse Response) : IRequest<string>;