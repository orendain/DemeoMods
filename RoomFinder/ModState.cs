namespace RoomFinder
{
    using Boardgame;

    internal class ModState
    {
        internal GameContext GameContext { get; set; }

        internal bool IsRefreshingRoomList { get; set; }

        internal bool HasRoomListUpdated { get; set; }

        internal static ModState NewInstance()
        {
            return new ModState();
        }

        private ModState()
        {
        }
    }
}
