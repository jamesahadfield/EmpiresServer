namespace EmpireServer.Services
{
    public class RoomManager : IRoomManager
    {
        private List<IGame> _rooms = new List<IGame>();
        public void CreateRoom(string roomName)
        {
            _rooms.Add(new Game(roomName));
        }

        public IGame? GetRoom(string roomName)
        {
            if (_rooms.Count == 0)
            {
                throw new Exception("No rooms have been created.");
            }
            if (_rooms.FirstOrDefault(r => r.RoomName == roomName) == null)
            {
                throw new Exception("Room does not exist.");
            }
            return _rooms.FirstOrDefault(r => r.RoomName == roomName);
        }
    }

    public interface IRoomManager
    {
        public void CreateRoom(string roomName);

        public IGame? GetRoom(string roomName);
    }
}
