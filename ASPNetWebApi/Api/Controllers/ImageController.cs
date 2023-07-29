using Application.Features.Image.GetChatImage;
using Application.Features.Image.UploadChatProfileImage;
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
    public async Task<IActionResult> UploadChatImage(IFormFile image, Guid chatId)
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
}