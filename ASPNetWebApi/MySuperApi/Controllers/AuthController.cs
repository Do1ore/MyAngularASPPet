using JWTModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySuperApi.DTOs;
using MySuperApi.JWTModule;
using MySuperApi.Models;
using MySuperApi.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MySuperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService, AppDbContext db)
        {
            _configuration = configuration;
            _userService = userService;
            _db = db;
        }

        [HttpGet("getme"), Authorize]
        public ActionResult<string> GetMe()
        {
            string userName = _userService.GetMyName();
            Response.ContentType = "text";
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterUserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            AppUser user = new()
            {
                Username = request.Username,
                Surname = request.Surname,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                AccountCreated = DateTime.UtcNow,
                AccountLastTimeEdited = DateTime.UtcNow,
                LastTimeOnline = DateTime.UtcNow,

            };


            if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Email exists");
            }
            _ = await _db.Users.AddAsync(user);
            _ = await _db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto dto)
        {
            if (dto is null)
            {
                return BadRequest("User data is null.");
            }
            AppUser? user = await _db.Users.SingleOrDefaultAsync(a => a.Email == dto.email);

            if (user is null)
            {
                return NotFound("Email not found.");
            }

            if (!VerifyPasswordHash(dto.password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            RefreshToken refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);

            return Ok(token);
        }

        [HttpGet("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            string userId = _userService.GetMyId();
            AppUser? user = await _db.Users.SingleOrDefaultAsync(a => a.Id == Guid.Parse(userId));
            if (user is null | string.IsNullOrEmpty(user?.Id.ToString()))
            {
                return NotFound("Email not found");
            }

            string? refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            RefreshToken newRefreshToken = GenerateRefreshToken();
            await SetRefreshToken(newRefreshToken, user);
            Response.ContentType = "application/json";
            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }

        private async Task SetRefreshToken(RefreshToken newRefreshToken, AppUser user)
        {
            CookieOptions cookieOptions = new()
            {
                Secure = true,
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                SameSite = SameSiteMode.None,
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            Response.ContentType = "application/json";
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            await _db.Users
                .Where(a => a.Id == user.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(a => a.RefreshToken, newRefreshToken.Token)
                .SetProperty(a => a.TokenCreated, newRefreshToken.Created)
                .SetProperty(a => a.TokenExpires, newRefreshToken.Expires));
        }

        private string CreateToken(AppUser user)
        {

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new(passwordSalt);
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}