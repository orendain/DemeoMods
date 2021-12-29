namespace RoomFinder
{
    internal class ModState
    {
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
