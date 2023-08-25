using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Image.UploadUserProfileImage;

public record UploadUserProfileImageRequest(IFormFile Image, Guid UserId) : IRequest<string>;