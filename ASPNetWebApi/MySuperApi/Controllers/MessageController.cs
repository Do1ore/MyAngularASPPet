using Microsoft.AspNetCore.Mvc;
using MySuperApi.Models;

namespace MySuperApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MessageController(AppDbContext db)
        {
            _db = db;
        }


    }
}
