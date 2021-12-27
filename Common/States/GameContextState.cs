using Boardgame;
using Boardgame.LevelLoading;
using Boardgame.Ui.LobbyMenu;
using Data.GameData;

namespace Common.States
{
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