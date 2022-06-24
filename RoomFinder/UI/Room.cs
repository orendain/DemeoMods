namespace RoomFinder.UI
{
    using Boardgame;
    using Photon.Realtime;

    internal class Room
    {
        private const string ModdedRoomPropertyKey = "modded";

        internal string Name { get; }

        internal LevelSequence.GameType GameType { get; }

        internal int Floor { get; }

        internal int CurrentPlayers { get; }

        internal int MaxPlayers { get; }

        internal bool IsModded { get; }

        internal static Room Parse(RoomInfo room)
        {
            var gameType = ExtractGameType(room);
            var floorIndex = ExtractFloorNumber(room);
            var isModded = ExtractModdedStatus(room);
            return NewInstance(room.Name, gameType, floorIndex, room.PlayerCount, room.MaxPlayers, isModded);
        }

        private static Room NewInstance(
            string name,
            LevelSequence.GameType gameType,
            int floor,
            int currentPlayers,
            int maxPlayers,
            bool isModded)
        {
            return new Room(name, gameType, floor, currentPlayers, maxPlayers, isModded);
        }

        private Room(
            string name,
            LevelSequence.GameType gameType,
            int floor,
            int currentPlayers,
            int maxPlayers,
            bool isModded)
        {
            Name = name;
            GameType = gameType;
            Floor = floor;
            CurrentPlayers = currentPlayers;
            MaxPlayers = maxPlayers;
            IsModded = isModded;
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

        private static bool ExtractModdedStatus(RoomInfo room)
        {
            return room.CustomProperties.TryGetValue(ModdedRoomPropertyKey, out var obj) && (bool)obj;
        }
    }
}
