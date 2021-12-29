namespace RoomFinder
{
    internal class ModState
    {
        public bool IsRefreshingRoomList { get; set; }

        public bool HasRoomListUpdated { get; set; }

        public static ModState NewInstance()
        {
            return new ModState();
        }

        private ModState()
        {
        }
    }
}
