using Application.Features.User;
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
    public async Task<IActionResult> GetUser(string searchTeam)
    {
        var userList = await _mediator.Send(new SearchUserRequest(searchTeam));
        return Ok(userList);
    }
}