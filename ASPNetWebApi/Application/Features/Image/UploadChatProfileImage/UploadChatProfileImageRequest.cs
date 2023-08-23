using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Image.UploadChatProfileImage;

public record UploadChatProfileImageRequest(IFormFile Image, Guid ChatId) : IRequest<string>;