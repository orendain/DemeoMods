namespace RoomFinder
{
    using Boardgame;

    internal class SharedState
    {
        internal GameContext GameContext { get; set; }

        internal bool IsRefreshingRoomList { get; set; }

        internal bool HasRoomListUpdated { get; set; }

        internal static SharedState NewInstance()
        {
            return new SharedState();
        }

        private SharedState()
        {
        }
    }
}
