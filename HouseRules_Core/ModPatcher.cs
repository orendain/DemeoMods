namespace HouseRules
{
    using System.Reflection;
    using Boardgame;
    using Boardgame.Networking;
    using Boardgame.SerializableEvents;
    using HarmonyLib;

    internal class ModPatcher
    {
        private static GameContext _gameContext;
        private static bool _isStartingGame;
        private static EventInterpreter _eventInterpreter;

        private static void InitEventInterpreter()
        {
            _eventInterpreter = EventInterpreter.NewInstance(_gameContext);
        }

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools
                    .Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo()
                    .GetDeclaredMethod("OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(CreatingGameState_OnJoinedRoom_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToPlayingState"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_GoToPlayingState_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PostGameControllerBase), "OnPlayAgainClicked"),
                postfix: new HarmonyMethod(
                    typeof(ModPatcher),
                    nameof(PostGameControllerBase_OnPlayAgainClicked_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "EndGame"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_EndGame_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "DisconnectLocalPlayer"),
                prefix: new HarmonyMethod(
                    typeof(ModPatcher),
                    nameof(SerializableEventQueue_DisconnectLocalPlayer_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "SendResponseEvent"),
                postfix: new HarmonyMethod(
                    typeof(ModPatcher),
                    nameof(SerializableEventQueue_SendResponseEvent_Postfix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _gameContext = gameContext;

            InitEventInterpreter();
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            if (_gameContext.gameStateMachine.goBackToMenuState)
            {
                return;
            }

            var createdGameFromSave =
                Traverse.Create(_gameContext.gameStateMachine).Field<bool>("createdGameFromSave").Value;
            if (createdGameFromSave)
            {
                return;
            }

            var createGameMode = Traverse.Create(_gameContext.gameStateMachine)
                .Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            HR.TriggerActivateRuleset(_gameContext, GameHub.GetGameMode);
            _isStartingGame = true;
            HR.TriggerPreGameCreated(_gameContext);
        }

        private static void GameStateMachine_GoToPlayingState_Postfix()
        {
            if (!_isStartingGame)
            {
                return;
            }

            _isStartingGame = false;
            HR.TriggerPostGameCreated(_gameContext);
            HR.TriggerWelcomeMessage();
        }

        private static void PostGameControllerBase_OnPlayAgainClicked_Postfix()
        {
            HR.TriggerActivateRuleset(_gameContext, GameHub.GetGameMode);
            _isStartingGame = true;
            HR.TriggerPreGameCreated(_gameContext);
        }

        private static void GameStateMachine_EndGame_Prefix()
        {
            HR.TriggerDeactivateRuleset(_gameContext);
        }

        private static void SerializableEventQueue_DisconnectLocalPlayer_Prefix()
        {
            HR.TriggerDeactivateRuleset(_gameContext);
        }

        private static void SerializableEventQueue_SendResponseEvent_Postfix(SerializableEvent serializableEvent)
        {
            if (!HR.IsRulesetActive)
            {
                return;
            }

            if (!_eventInterpreter.DoesEventRepresentNewSpawn(serializableEvent))
            {
                return;
            }

            _gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
        }
    }
}
