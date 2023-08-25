using Infrastructure.Abstraction.Services;
using MediatR;

namespace Application.Features.Image.UploadUserProfileImage;

public class UploadUserProfileImageRequestHandler : IRequestHandler<UploadUserProfileImageRequest, string>
{
    private readonly IImageService _imageService;

    public UploadUserProfileImageRequestHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<string> Handle(UploadUserProfileImageRequest request, CancellationToken cancellationToken)
    {
        var path = await _imageService.UploadUserProfileImage(request.Image, request.UserId);
        return path;
    }
}