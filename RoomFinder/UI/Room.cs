namespace RoomFinder.UI
{
    using Boardgame;
    using Photon.Realtime;

    internal class Room
    {
        internal string Name { get; }

        internal LevelSequence.GameType GameType { get; }

        internal int Floor { get; }

        internal int CurrentPlayers { get; }

        internal int MaxPlayers { get; }

        internal static Room Parse(RoomInfo room)
        {
            var gameType = ExtractGameType(room);
            var floorIndex = ExtractFloorNumber(room);
            return NewInstance(room.Name, gameType, floorIndex, room.PlayerCount, room.MaxPlayers);
        }

        private static Room NewInstance(
            string name,
            LevelSequence.GameType gameType,
            int floor,
            int currentPlayers,
            int maxPlayers)
        {
            return new Room(name, gameType, floor, currentPlayers, maxPlayers);
        }

        private Room(string name, LevelSequence.GameType gameType, int floor, int currentPlayers, int maxPlayers)
        {
            Name = name;
            GameType = gameType;
            Floor = floor;
            CurrentPlayers = currentPlayers;
            MaxPlayers = maxPlayers;
        }

        private static LevelSequence.GameType ExtractGameType(RoomInfo room)
        {
            return room.CustomProperties.TryGetValue("at", out var obj)
                ? (LevelSequence.GameType)obj
                : LevelSequence.GameType.Invalid;
        }

        private static int ExtractFloorNumber(RoomInfo room)
        {
            return room.CustomProperties.TryGetValue("fi", out var obj) ? (int)obj : -1;
        }
    }
}
