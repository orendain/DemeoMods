namespace Common.States
{
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
    using Data.GameData;

    internal static class GameContextState
    {
        public static GameContext GameContext { get; set; }

        public static AvatarController AvatarController { get; set; }

        public static ICardHandController CardHandController { get; set; }

        public static GameDataAPI GameDataAPI { get; set; }

        public static LobbyMenuController LobbyMenuController { get; set; }

        public static PieceAndTurnController PieceAndTurnController { get; set; }
    }
}
