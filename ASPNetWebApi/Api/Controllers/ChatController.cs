using Application.Features.Chat.ChatDetails;
using Application.Features.Chat.CreateChat;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MySuperApi.Controllers;

public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat(CreateChatDto createChatDto)
    {
        var chat = await _mediator.Send(new CreateChatRequest(createChatDto));
        return Ok(chat);
    }

    [HttpGet("details")]
    public async Task<IActionResult> GetChatDetails(Guid chatId, Guid userId)
    {
        var chat = await _mediator.Send(new ChatDetailsRequest(userId, chatId));
        return Ok(chat);
    }
}