namespace EmpireServer.Services
{
    public class Game : IGame
    {
        private readonly string _roomName;
        private Dictionary<string, string> _players = new Dictionary<string, string>();

        public Game(string roomName)
        {
            _roomName = roomName;
        }

        public string RoomName => _roomName;

        public async Task Join(string playerName, string given)
        {
            _players.Add(playerName, given);
        }

        public async Task<bool> MakeGuess(string playerName, string guess)
        {
            return _players.ContainsKey(playerName) && _players[playerName] == guess;
        }
    }

    public interface IGame
    {
        public Task Join(string playerName, string given);
        public Task<bool> MakeGuess(string playerName, string guess);
        public string RoomName { get; }
    }
}
