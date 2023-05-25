using Microsoft.AspNetCore.SignalR;

namespace MySuperApi.HubConfig
{
    public class UserHub : Hub
    {
        private readonly ILogger<UserHub> _logger;

        public UserHub(ILogger<UserHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("New connection: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Connection " + Context.ConnectionId + " terminated");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
