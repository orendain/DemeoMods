namespace RulesAPI
{
    using System.Reflection;
    using Boardgame;
    using Boardgame.Networking;
    using HarmonyLib;

    internal class ModPatcher
    {
        private static GameContext _gameContext;
        private static bool _isStartingGame;

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo().GetDeclaredMethod("OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(CreatingGameState_OnJoinedRoom_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToPlayingState"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_GoToPlayingState_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "EndGame"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_EndGame_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "DisconnectLocalPlayer"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(SerializableEventQueue_DisconnectLocalPlayer_Prefix)));
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

            RulesAPI.ActivateSelectedRuleset();

            _isStartingGame = true;
            RulesAPI.TriggerPreGameCreated();
        }

        private static void GameStateMachine_GoToPlayingState_Postfix()
        {
            if (!_isStartingGame)
            {
                return;
            }

            _isStartingGame = false;
            RulesAPI.TriggerPostGameCreated();
        }

        private static void GameStateMachine_EndGame_Prefix()
        {
            RulesAPI.DeactivateSelectedRuleset();
        }

        private static void SerializableEventQueue_DisconnectLocalPlayer_Prefix()
        {
            RulesAPI.DeactivateSelectedRuleset();
        }
    }
}
