using MediatR;

namespace Application.Features.Image.GetChatProfileImage;

public record GetChatImageRequest(Guid ChatId) : IRequest<string>;