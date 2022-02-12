namespace HouseRules
{
    using System.Reflection;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.Networking;
    using Boardgame.SerializableEvents;
    using DataKeys;
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

            if (!DoesEventRepresentNewSpawn(serializableEvent))
            {
                return;
            }

            _gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
        }

        private static bool DoesEventRepresentNewSpawn(SerializableEvent serializableEvent)
        {
            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.SpawnPiece:
                case SerializableEvent.Type.UpdateFogAndSpawn:
                case SerializableEvent.Type.SetBoardPieceID:
                case SerializableEvent.Type.SlimeFusion:
                case SerializableEvent.Type.GoToNextLevel:
                    return true;
            }

            if (serializableEvent.type == SerializableEvent.Type.OnAbilityUsed)
            {
                return DoesAbilityEventRepresentNewSpawn((SerializableEventOnAbilityUsed)serializableEvent);
            }

            if (serializableEvent.type == SerializableEvent.Type.PieceDied)
            {
                return DoesPieceDiedEventRepresentNewSpawn((SerializableEventPieceDied)serializableEvent);
            }

            return false;
        }

        private static bool DoesAbilityEventRepresentNewSpawn(SerializableEventOnAbilityUsed onAbilityUsedEvent)
        {
            var abilityKey = Traverse.Create(onAbilityUsedEvent).Field<AbilityKey>("abilityKey").Value;
            switch (abilityKey)
            {
                case AbilityKey.SummonElemental:
                case AbilityKey.SummonBossMinions:
                case AbilityKey.NaturesCall:
                case AbilityKey.Tornado:
                case AbilityKey.MonsterBait:
                case AbilityKey.ProximityMine:
                case AbilityKey.EyeOfAvalon:
                case AbilityKey.SwordOfAvalon:
                case AbilityKey.BeaconOfSmite:
                case AbilityKey.BeaconOfHealing:
                case AbilityKey.RaiseRoots:
                case AbilityKey.CallCompanion:
                    return true;
            }

            var abilityName = abilityKey.ToString();
            var isSpawnAbility = abilityName.Contains("Spawn");
            var isLampAbility = abilityName.Contains("Lamp");

            return isSpawnAbility || isLampAbility;
        }

        private static bool DoesPieceDiedEventRepresentNewSpawn(SerializableEventPieceDied pieceDiedEvent)
        {
            foreach (var pieceId in pieceDiedEvent.deadPieces)
            {
                if (!_gameContext.pieceAndTurnController.TryGetPiece(pieceId, out Piece piece))
                {
                    continue;
                }

                if (piece.boardPieceId == BoardPieceId.SpiderEgg)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
