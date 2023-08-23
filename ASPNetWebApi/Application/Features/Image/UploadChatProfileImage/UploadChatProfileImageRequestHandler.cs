using Infrastructure.Abstraction.Services;
using MediatR;

namespace Application.Features.Image.UploadChatProfileImage;

public class UploadChatProfileImageRequestHandler : IRequestHandler<UploadChatProfileImageRequest, string>
{
    private readonly IImageService _imageService;

    public UploadChatProfileImageRequestHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<string> Handle(UploadChatProfileImageRequest request, CancellationToken cancellationToken)
    {
        var path = await _imageService.UploadChatProfileImage(request.Image, request.ChatId);
        return path;
    }
}