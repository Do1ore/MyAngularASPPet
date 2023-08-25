using Infrastructure.Abstraction.Services;
using MediatR;

namespace Application.Features.Image.GetChatProfileImage;

public class GetChatImageRequestHandler : IRequestHandler<GetChatImageRequest, string>
{
    private readonly IImageService _imageService;

    public GetChatImageRequestHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<string> Handle(GetChatImageRequest request, CancellationToken cancellationToken)
    {
        return await _imageService.GetChatProfileImage(request.ChatId);
    }
}