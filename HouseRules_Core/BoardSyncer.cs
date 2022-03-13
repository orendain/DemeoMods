﻿namespace HouseRules
{
    using System;
    using Boardgame;
    using Boardgame.BoardEntities;
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
        private static bool _isStatusImmunitiesTouched;
        private static bool _isStatusEffectsTouched;

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

            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "IsImmuneToStatusEffect"),
                postfix: new HarmonyMethod(
                    typeof(BoardSyncer),
                    nameof(Piece_IsImmuneToStatusEffect_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(EffectSink), "AddStatusEffect"),
                postfix: new HarmonyMethod(
                    typeof(BoardSyncer),
                    nameof(EffectSink_AddStatusEffect_Postfix)));
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

            if (HR.SelectedRuleset.ModifiedData == SpecialSyncData.None)
            {
                return;
            }

            UpdateSyncTriggers(serializableEvent);

            if (!IsSyncNeeded())
            {
                return;
            }

            if (!IsSyncOpportunity(serializableEvent))
            {
                return;
            }

            SyncBoard();
        }

        private static void Piece_IsImmuneToStatusEffect_Postfix()
        {
            _isStatusImmunitiesTouched = true;
        }

        private static void EffectSink_AddStatusEffect_Postfix()
        {
            _isStatusEffectsTouched = true;
        }

        private static void UpdateSyncTriggers(SerializableEvent serializableEvent)
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
                case AbilityKey.MiniBarricade:
                case AbilityKey.MagicWall:
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

        private static bool IsSyncNeeded()
        {
            var hasSyncType = (HR.SelectedRuleset.ModifiedData & SpecialSyncData.PieceData) > 0;
            if (hasSyncType && _isNewSpawnPossible)
            {
                return true;
            }

            hasSyncType = (HR.SelectedRuleset.ModifiedData & SpecialSyncData.StatusEffectImmunity) > 0;
            if (hasSyncType && _isStatusImmunitiesTouched)
            {
                return true;
            }

            hasSyncType = (HR.SelectedRuleset.ModifiedData & SpecialSyncData.StatusEffectData) > 0;
            if (hasSyncType && _isStatusEffectsTouched)
            {
                return true;
            }

            return false;
        }

        private static bool IsSyncOpportunity(SerializableEvent serializableEvent)
        {
            if (!_gameContext.pieceAndTurnController.IsPlayersTurn())
            {
                return serializableEvent.type == SerializableEvent.Type.EndTurn;
            }

            return true;
        }

        private static void SyncBoard()
        {
            _isNewSpawnPossible = false;
            _isStatusImmunitiesTouched = false;
            _isStatusEffectsTouched = false;
            _gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
        }
    }
}
