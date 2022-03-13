namespace HouseRules
{
    using System;
    using Boardgame;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;

    [Flags]
    public enum SpecialSyncData
    {
        None = 0,
        PieceData = 1,
        StatusEffectImmunity = 2,
        StatusEffectData = 4,
    }

    internal static class BoardSyncer
    {
        private static GameContext _gameContext;
        private static bool _isNewSpawnPossible;

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(BoardSyncer), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "SendResponseEvent"),
                postfix: new HarmonyMethod(
                    typeof(BoardSyncer),
                    nameof(SerializableEventQueue_SendResponseEvent_Postfix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _gameContext = gameContext;
        }

        private static void SerializableEventQueue_SendResponseEvent_Postfix(SerializableEvent serializableEvent)
        {
            if (!HR.IsRulesetActive)
            {
                return;
            }

            UpdateTriggers(serializableEvent);

            if (!_isNewSpawnPossible)
            {
                return;
            }

            TriggerBoardSync();
        }

        private static void TriggerBoardSync()
        {
            _isNewSpawnPossible = false;
            _gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
        }

        private static void UpdateTriggers(SerializableEvent serializableEvent)
        {
            if (CanEventRepresentNewSpawn(serializableEvent))
            {
                _isNewSpawnPossible = true;
            }
        }

        private static bool CanEventRepresentNewSpawn(SerializableEvent serializableEvent)
        {
            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.SpawnPiece:
                case SerializableEvent.Type.UpdateFogAndSpawn:
                case SerializableEvent.Type.SetBoardPieceID:
                case SerializableEvent.Type.SlimeFusion:
                case SerializableEvent.Type.GoToNextLevel:
                    return true;
                case SerializableEvent.Type.OnAbilityUsed:
                    return CanAbilityEventRepresentNewSpawn((SerializableEventOnAbilityUsed)serializableEvent);
                case SerializableEvent.Type.PieceDied:
                    return CanPieceDiedEventRepresentNewSpawn((SerializableEventPieceDied)serializableEvent);
                default:
                    return false;
            }
        }

        private static bool CanAbilityEventRepresentNewSpawn(SerializableEventOnAbilityUsed onAbilityUsedEvent)
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
                case AbilityKey.DigRatsNest:
                    return true;
            }

            var abilityName = abilityKey.ToString();
            var isSpawnAbility = abilityName.Contains("Spawn");
            var isLampAbility = abilityName.Contains("Lamp");

            return isSpawnAbility || isLampAbility;
        }

        private static bool CanPieceDiedEventRepresentNewSpawn(SerializableEventPieceDied pieceDiedEvent)
        {
            foreach (var pieceId in pieceDiedEvent.deadPieces)
            {
                if (!_gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
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
