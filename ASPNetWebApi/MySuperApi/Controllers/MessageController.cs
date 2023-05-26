using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using MySuperApi.Models;
using MySuperApi.Repositories.Implementation;
using MySuperApi.Repositories.Interfaces;
using NuGet.Protocol.Plugins;

namespace MySuperApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IChatRepository _chatRepository;
        public MessageController(AppDbContext db, IChatRepository chatRepository)
        {
            _db = db;
            this._chatRepository = chatRepository;
        }

        [HttpPost("test")]

        public async Task<IActionResult> TestAsync(string userId)
        {
            var chats = await _chatRepository.GetAllChatsForUser(userId.ToString());
            return Ok(chats);
        }

        [HttpPost("test-message")]

        public async Task<IActionResult> TestMessageAsync(string chatId, string senderId, string messageContent)
        {
            await _chatRepository.SendMessage(chatId, senderId, messageContent);
            return Ok();
        }



    }
}
