using Application.Features.Image.GetChatProfileImage;
using Application.Features.Image.GetUserProfileImage;
using Application.Features.Image.UploadChatProfileImage;
using Application.Features.Image.UploadUserProfileImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace MySuperApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload-chat-image")]
    public async Task<IActionResult> UploadChatImage([FromForm] IFormFile image, [FromForm] Guid chatId)
    {
        var result = await _mediator.Send(new UploadChatProfileImageRequest(image, chatId));
        return Ok(result);
    }

    [HttpGet("get-chat-image/{chatId}")]
    public async Task<IActionResult> GetChatImage(Guid chatId)
    {
        var result = await _mediator.Send(new GetChatImageRequest(chatId));
        return PhysicalFile(result, "image/jpeg");
    }

    [HttpPost("upload-user-image")]
    public async Task<IActionResult> UploadUserProfileImage(IFormFile image, Guid userid)
    {
        var result = await _mediator.Send(new UploadUserProfileImageRequest(image, userid));
        return Ok(result);
    }

    [HttpGet("get-user-image/{userId}")]
    public async Task<IActionResult> GetUserProfileImage(Guid userId)
    {
        var result = await _mediator.Send(new GetUserProfileImageRequest(userId));
        return PhysicalFile(result, "image/jpeg");
    }
}