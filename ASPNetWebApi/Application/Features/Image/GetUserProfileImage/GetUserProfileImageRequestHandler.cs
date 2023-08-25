using Infrastructure.Abstraction.Services;
using MediatR;

namespace Application.Features.Image.GetUserProfileImage;

public class GetUserProfileImageRequestHandler : IRequestHandler<GetUserProfileImageRequest, string>
{
    private readonly IImageService _imageService;

    public GetUserProfileImageRequestHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public Task<string> Handle(GetUserProfileImageRequest request, CancellationToken cancellationToken)
    {
        return _imageService.GetUserProfileImage(request.UserId);
    }
}