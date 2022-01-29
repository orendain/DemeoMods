namespace RulesAPI
{
    using System.Reflection;
    using Boardgame;
    using Boardgame.BoardgameActions;
    using Boardgame.Networking;
    using HarmonyLib;

    internal class ModPatcher
    {
        private static GameContext _gameContext;

        internal static void Patch(Harmony harmony)
        {

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo().GetDeclaredMethod("OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(CreatingGameState_OnJoinedRoom_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(BoardGameActionStartNewGame), "StartNewGame"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(BoardGameActionStartNewGame_StartNewGame_Postfix)));

            // TODO(orendain): Hook into game ending events in order to deactivate activated rules.
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _gameContext = gameContext;
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            if (_gameContext.gameStateMachine.goBackToMenuState)
            {
                return;
            }

            var createdGameFromSave = Traverse.Create(_gameContext.gameStateMachine).Field<bool>("createdGameFromSave").Value;
            if (createdGameFromSave)
            {
                return;
            }

            var createGameMode = Traverse.Create(_gameContext.gameStateMachine).Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            // TODO(orendain): Call rules' OnPreGameCreated().
        }

        private static void BoardGameActionStartNewGame_StartNewGame_Postfix()
        {
            RulesAPI.ActivateSelectedRuleset();
        }
    }
}
