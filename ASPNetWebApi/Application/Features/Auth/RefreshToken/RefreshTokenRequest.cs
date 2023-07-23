using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.RefreshToken;

public record RefreshTokenRequest(Guid UserId, HttpRequest HttpRequest, HttpResponse HttpResponse) : IRequest<string>;