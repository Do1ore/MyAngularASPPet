using Microsoft.AspNetCore.Mvc;
using MySuperApi.Infrastructure.Repositories.Interfaces;
using MySuperApi.Infrastructure.Repositories.Services.JWTModule;

namespace MySuperApi.Api.Controllers;

[ApiController]
public class Test : ControllerBase
{
    private readonly IUserRepository _repository;

    public Test(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("user-add-to-mongo")]
    public async Task<IActionResult> Index(RegisterUserDto registerUserDto)
    {
        var result = await _repository.AddUser(registerUserDto);
        return Ok(result);
    }
}