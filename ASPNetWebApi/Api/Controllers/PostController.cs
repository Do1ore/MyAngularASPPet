using Application.Features.User.GetUserPosts;
using Domain;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MySuperApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{

    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-user-posts/{userId}")]
    public async Task<IActionResult> GetUserPostsById(Guid userId)
    {
        var posts = await _mediator.Send(new GetUserPostsRequest(userId));
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(Guid userId, CreatePostDto createPostDto)
    {
        //TODO 
        return Ok();
    }
}