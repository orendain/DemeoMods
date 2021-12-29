namespace Common.Patches
{
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
    using HarmonyLib;

    internal static class GameContextPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(GameContextPatcher), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: typeof(Lobby).GetMethod("Init"),
                postfix: new HarmonyMethod(typeof(GameContextPatcher), nameof(Lobby_Init_Postfix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            CommonModule.GameContextState.GameContext = gameContext;
            CommonModule.GameContextState.AvatarController = gameContext.avatarController;
            CommonModule.GameContextState.CardHandController = gameContext.cardHandController;
            CommonModule.GameContextState.GameDataAPI = gameContext.gameDataAPI;
            CommonModule.GameContextState.PieceAndTurnController = gameContext.pieceAndTurnController;
            CommonModule.Logger.Msg("Captured GameContext values.");
        }

        private static void Lobby_Init_Postfix(LobbyMenuController lobbyMenuController)
        {
            CommonModule.GameContextState.LobbyMenuController = lobbyMenuController;
            CommonModule.Logger.Msg("Captured LobbyMenuController.");
        }
    }
}
