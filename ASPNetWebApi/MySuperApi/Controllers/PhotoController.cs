using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperApi.Models.APIModels;
using MySuperApi.Services.PathLogic;
using System.Security.Permissions;

namespace MySuperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly AppDbContext db;
        private readonly IPathMaster pathMaster;
        public PhotoController(IWebHostEnvironment environment, AppDbContext db, IPathMaster pathMaster)
        {
            this.environment = environment;
            this.db = db;
            this.pathMaster = pathMaster;
        }

        [HttpGet("AllImages")]
        public async Task<IActionResult> GetImagesAsync()
        {
            var a = await LoadImagesAsync();
            return Ok(await db.ApiImages.ToListAsync());
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile photo)
        {

            if (photo == null || photo.Length == 0)
            {
                return BadRequest("Photo is not selected");
            }
            var path = pathMaster.CreatePath(photo.FileName);

            using (var image = await Image.LoadAsync(photo.OpenReadStream()))
            {
                int size = Math.Min(image.Width, image.Height);
                //crop image
                image.Mutate(x => x
                    .Crop(new Rectangle((image.Width - size) / 2, (image.Height - size) / 2, size, size))
                    .AutoOrient()
                    .Saturate(0.5f));
                image.Save(environment.WebRootPath + "/" + path);
                ApiImageModel model = new ApiImageModel()
                {
                    FileName = pathMaster.CreateFileName(photo.FileName),
                    ImagePath = path,
                };
                await db.ApiImages.AddAsync(model);
                await db.SaveChangesAsync();
            }
            return Ok();
        }


        private async Task<IEnumerable<Image>> LoadImagesAsync()
        {
            var imagePaths = Directory.GetFiles(Path.Combine(environment.ContentRootPath, "images", "uploaded", "/")); // получаем пути к файлам изображений

            var tasks = imagePaths.Select(async imagePath =>
            {
                using var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true); // открываем поток для чтения файла изображения
                return await Image.LoadAsync(fileStream); // асинхронно загружаем изображение
            });

            return await Task.WhenAll(tasks); // ждём, пока все задачи загрузки завершатся, и возвращаем массив изображений
        }

        //[HttpPost("EditImage")]
        //public async Task<IActionResult> EditImage([FromBody] IFormFile photo)
        //{
        //    if (photo == null || photo.Length == 0)
        //    {
        //        return BadRequest("Photo is not selected");
        //    }
        //    var path = pathMaster.CreatePath(photo.FileName);
        //    using (var image = await Image.LoadAsync(photo.OpenReadStream()))
        //    {
        //        int size = Math.Min(image.Width, image.Height);
        //        //crop image
        //        image.Mutate(x => x
        //            .Crop(new Rectangle((image.Width - size) / 2, (image.Height - size) / 2, size, size))
        //            .AutoOrient()
        //            .Saturate(0.5f));
        //        ApiImageModel model = new ApiImageModel()
        //        {
        //            FileName = photo.FileName,
        //            ImagePath = path,
        //        };

        //        await db.AddAsync(model);
        //        await db.SaveChangesAsync();

        //        image.Save(Path.Combine(environment.ContentRootPath, path));
        //    }
        //    return Ok();
        //}


    }

}
