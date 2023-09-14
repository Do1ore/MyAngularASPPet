using Application.Features.User.GetUserById;
using Application.Features.User.SearchUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MySuperApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search-user/{searchTeam}")]
    public async Task<IActionResult> GetUsers(string searchTeam)
    {
        var userList = await _mediator.Send(new SearchUserRequest(searchTeam));
        return Ok(userList);
    }

    [HttpGet("get-user/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _mediator.Send(new GetUserByIdRequest(userId));
        return Ok(user);
    }

}