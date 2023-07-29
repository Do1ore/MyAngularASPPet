using MediatR;

namespace Application.Features.Image.GetChatImage;

public record GetChatImageRequest(Guid ChatId) : IRequest<string>;