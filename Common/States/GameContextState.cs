namespace Common.States
{
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
    using Data.GameData;

    internal class GameContextState
    {
        public GameContext GameContext { get; set; }

        public AvatarController AvatarController { get; set; }

        public ICardHandController CardHandController { get; set; }

        public GameDataAPI GameDataAPI { get; set; }

        public LobbyMenuController LobbyMenuController { get; set; }

        public PieceAndTurnController PieceAndTurnController { get; set; }

        public static GameContextState NewInstance()
        {
            return new GameContextState();
        }

        private GameContextState()
        {
        }
    }
}
