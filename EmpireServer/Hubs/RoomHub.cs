using EmpireServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace EmpireServer.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomManager _roomManager;
        public RoomHub(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }
        public async Task NewMessage(long username, string message) =>
            await Clients.All.SendAsync("messageReceived", username, message);

        public async Task CreateRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            _roomManager.CreateRoom(roomName);
            await Clients.Group(roomName).SendAsync("roomCreated", roomName);
        }
        public async Task JoinRoom(string roomName, string playerName, string givenName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            var room = _roomManager.GetRoom(roomName);
            if (room == null)
            {
                await Clients.Caller.SendAsync("roomNotFound", roomName);
                return;
            }
            await room.Join(playerName, givenName);
            await Clients.Group(roomName).SendAsync("playerJoined", playerName, givenName);
        }

        public async Task MakeGuess(string roomName, string playerName, string guess)
        {
            var room = _roomManager.GetRoom(roomName);
            if (room == null)
            {
                await Clients.Caller.SendAsync("roomNoLongerExists", roomName);
                return;
            }
            await Clients.Group(roomName).SendAsync("guessReceived", playerName, guess);
            if (!await room.MakeGuess(playerName, guess))
            {
                await Clients.Caller.SendAsync("guessFailed", playerName, guess);
                return;
            }
            await Clients.Group(roomName).SendAsync("guessSucceeded", playerName, guess);
        }
    }
}
