using MediatR;

namespace Application.Features.Image.GetUserProfileImage;

public record GetUserProfileImageRequest(Guid UserId) : IRequest<string>;