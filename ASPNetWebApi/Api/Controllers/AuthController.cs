using Application.Features.Auth.RefreshToken;
using Application.Features.Auth.RegisterUser;
using Application.Features.Auth.SignIn;
using Domain.DTOs;
using Infrastructure.Abstraction.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegisterUserDto = Domain.JWTModels.RegisterUserDto;

namespace MySuperApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHttpUserDataAccessorService _httpUserDataAccessorService;
    private readonly IMediator _mediator;

    public AuthController(IConfiguration configuration,
        IHttpUserDataAccessorService httpUserDataAccessorService,
        IMediator mediator)
    {
        _configuration = configuration;
        _httpUserDataAccessorService = httpUserDataAccessorService;
        _mediator = mediator;
    }

    [HttpGet("getme"), Authorize]
    public IActionResult GetMe()
    {
        string userName = _httpUserDataAccessorService.GetMyName();
        Response.ContentType = "text";
        return Ok(userName);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto request)
    {
        var user = await _mediator.Send(new RegisterRequest(request));
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto dto)
    {
        var token = await _mediator.Send(new SingInRequest(dto, Response));
        return Ok(token);
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var userId = Guid.Parse(_httpUserDataAccessorService.GetMyId());
        var token = await _mediator.Send(new RefreshTokenRequest(userId, Request, Response));
        return Ok(token);
    }
}