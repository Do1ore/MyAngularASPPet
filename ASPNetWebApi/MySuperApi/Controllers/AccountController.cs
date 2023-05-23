using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperApi.Models;
using MySuperApi.Services.PathLogic;
using MySuperApi.Services.UserService;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace MySuperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPathMaster _pathMaster;
        private readonly AppDbContext _db;


        public AccountController(IPathMaster pathMaster, IUserService userService, AppDbContext db)
        {
            _pathMaster = pathMaster;
            _userService = userService;
            _db = db;
        }



        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync()
        {
            var userId = _userService.GetMyId();
            var a = _userService.GetMyName();
            var user = await _db.Users
                .Include(a => a.UserProfileImages)
                .FirstOrDefaultAsync(i => i.Id.ToString() == userId);
            try
            {
                var file = Request.Form.Files[0];
                string folderName = Path.Combine("Resourses", "Uploaded");
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory());

                if (file.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, folderName, filename);
                    var relativePath = Path.Combine(folderName, filename);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new JsonResult("Uploaded"));
                }
                else
                {
                    return BadRequest();

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("load-image"), DisableRequestSizeLimit]
        public async Task<IActionResult> AddImage(IFormFile formData)
        {
            var userId = _userService.GetMyId();
            var user = await _db.Users
                .Include(a => a.UserProfileImages)
                .FirstOrDefaultAsync(i => i.Id.ToString() == userId);

            if (formData != null)
            {
                var path = _pathMaster.CreatePath(formData.FileName);
                user?.UserProfileImages.Add(
                    new UserProfileImage()
                    {
                        ImagePath = path,
                        FileName = formData.FileName,
                        AppUserId = Guid.Parse(userId),
                    });


            }

            return Ok();
        }
    }
}
