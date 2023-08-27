using Application.Features.Image.GetChatProfileImage;
using Application.Features.Image.GetUserProfileImage;
using Application.Features.Image.UploadChatProfileImage;
using Application.Features.Image.UploadUserProfileImage;
using Infrastructure.Abstraction.Services.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MySuperApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpUserDataAccessorService _dataAccessorService;

    public ImageController(IMediator mediator, IHttpUserDataAccessorService dataAccessorService)
    {
        _mediator = mediator;
        _dataAccessorService = dataAccessorService;
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
    public async Task<IActionResult> UploadUserProfileImage([FromForm] IFormFile image, [FromForm] Guid userid)
    {
        var result = await _mediator.Send(new UploadUserProfileImageRequest(image, userid));
        return Ok(result);
    }

    [HttpGet("get-current-user-image")]
    public async Task<IActionResult> GetCurrentUserProfileImage()
    {
        var userId = _dataAccessorService.GetMyId();
        var result = await _mediator.Send(new GetUserProfileImageRequest(Guid.Parse(userId)));
        return PhysicalFile(result, "image/jpeg");
    }

    [HttpGet("get-user-image/{userId}")]
    public async Task<IActionResult> GetUserProfileImage(Guid userId)
    {
        var result = await _mediator.Send(new GetUserProfileImageRequest(userId));
        return PhysicalFile(result, "image/jpeg");
    }
}