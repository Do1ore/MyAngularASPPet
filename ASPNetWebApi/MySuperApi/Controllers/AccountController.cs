using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperApi.Models;
using MySuperApi.Repositories.Interfaces;
using MySuperApi.Services.PathLogic;
using MySuperApi.Services.ProfileImageService;
using MySuperApi.Services.UserService;
using System.Net.Http.Headers;

namespace MySuperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IChatRepository _chatResository;
        private readonly IPathMaster _pathMaster;
        private readonly IProfileImageService _userProfileImage;
        private readonly AppDbContext _db;


        public AccountController(IPathMaster pathMaster, IUserService userService, AppDbContext db, IProfileImageService userProfileImage, IChatRepository chatResository)
        {
            _pathMaster = pathMaster;
            _userService = userService;
            _userProfileImage = userProfileImage;
            _db = db;
            _chatResository = chatResository;
        }



        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync()
        {
            var userId = _userService.GetMyId();
            var a = _userService.GetMyName();
            var user = await _db.Users
                .Include(a => a.UserProfileImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id.ToString() == userId);
            if (user == null)
            {
                return Unauthorized("User not found");
            }


            var file = Request.Form.Files[0];
            string folderName = Path.Combine("Resourses", "Uploaded");
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory());

            if (file.Length > 0)
            {
                var filename = Guid.NewGuid().ToString()
                    + DateTime.Now.ToShortDateString()
                    + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');

                var fullPath = Path.Combine(pathToSave, folderName, filename);
                var relativePath = Path.Combine(folderName, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var profileImage = _userProfileImage.CreateProfileImageModel(filename, relativePath, userId);

                await _db.ProfileImages.AddAsync(profileImage);
                await _db.ProfileImageStorages.AddAsync(new UserProfileImageStorage()
                {
                    UserId = Guid.Parse(userId),
                    ProfileImage = profileImage
                });
                await _db.SaveChangesAsync();
                await _chatResository.UpdateCurrentProfileImage(profileImage.ImageId.ToString(), userId);

                return Ok(new JsonResult("Uploaded"));
            }
            else
            {
                return BadRequest();

            }
        }

        [HttpGet("profile-image")]
        public async Task<IActionResult> GetProfileImage()
        {
            var userId = _userService.GetMyId();
            var a = _userService.GetMyName();
            var user = await _db.Users
                .Include(a => a.UserProfileImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id.ToString() == userId);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var imagePath = await _chatResository.GetProfileImage(userId);
            if (string.IsNullOrEmpty(imagePath))
            {
                return BadRequest("No image found.");
            }
            string pathToImage = Path.Combine(Directory.GetCurrentDirectory(), imagePath);
            if (!System.IO.File.Exists(pathToImage))
            {
                return BadRequest("No image file found.");
            }
            var file = System.IO.File.ReadAllBytes(pathToImage);
            Response.Headers.Add("Content-Type", "image/png");

            return File(file, "image/png");
        }
    }
}
