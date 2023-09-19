namespace RoomFinder
{
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;

    internal class SharedState
    {
        internal GameContext GameContext { get; set; }

        internal LobbyMenuController LobbyMenuController { get; set; }

        internal LobbyMatchmakingController LobbyMatchmakingController { get; set; }

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
