using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySuperApi.Models;

namespace MySuperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly MyPetContext db;

        public UserController(MyPetContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await db.AspNetUsers.ToListAsync());
        }

    }
}
