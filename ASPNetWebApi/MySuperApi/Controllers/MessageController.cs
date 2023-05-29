using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySuperApi.HubConfig;
using MySuperApi.Models;
using MySuperApi.Models.MessageModels;
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
        private readonly IHubContext<UserHub> _hubContext;

        public MessageController(AppDbContext db, IChatRepository chatRepository, IHubContext<UserHub> hubContext)
        {
            _db = db;
            _chatRepository = chatRepository;
            _hubContext = hubContext;
        }

        [HttpPost("sendmodel")]
        public async Task<IActionResult> SendModelToClient(string connectionId, Chat model)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveModel", model);
            return Ok();
        }

       

   

        [HttpPost("test-message")]

        public async Task<IActionResult> TestMessageAsync(string chatId, string senderId, string messageContent)
        {
            await _chatRepository.SendMessage(chatId, senderId, messageContent);
            return Ok();
        }



    }
}
