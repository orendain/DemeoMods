namespace HouseRules
{
    using System;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;

    [Flags]
    public enum SyncableTrigger
    {
        /// <summary>Nothing that needs special syncing is modified.</summary>
        None = 0,

        /// <summary>New pieces are changed or have their attributes modified.</summary>
        NewPieceModified = 1,

        /// <summary>Status effect immunities are modified.</summary>
        StatusEffectImmunityModified = 2,

        /// <summary>Status effect data (e.g., values, duration, etc.) are modified.</summary>
        StatusEffectDataModified = 4,
    }

    internal static class BoardSyncer
    {
        private static GameContext _gameContext;
        private static bool _isSyncScheduled;
        private static string _reason;
        private static string _reason2;
        /// <summary>
        /// Schedules a sync to be triggered at the next available opportunity.
        /// </summary>
        internal static void ScheduleSync()
        {
            if (!HR.IsRulesetActive)
            {
                throw new InvalidOperationException("Can not schedule sync without an active ruleset.");
            }

            _isSyncScheduled = true;
        }

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

            if (!_isSyncScheduled && CanRepresentNewSpawn(serializableEvent))
            {
                _isSyncScheduled = true;
            }

            if (_isSyncScheduled && IsSyncOpportunity(serializableEvent))
            {
                SyncBoard();
            }
        }

        private static void Piece_IsImmuneToStatusEffect_Postfix()
        {
            var isEffectImmunityCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.StatusEffectImmunityModified) > 0;
            if (isEffectImmunityCheckRequired)
            {
                _isSyncScheduled = true;
            }
        }

        private static void EffectSink_AddStatusEffect_Postfix()
        {
            var isEffectDataCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.StatusEffectDataModified) > 0;
            if (isEffectDataCheckRequired)
            {
                _isSyncScheduled = true;
            }
        }

        private static bool CanRepresentNewSpawn(SerializableEvent serializableEvent)
        {
            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.SpawnPiece:
                    _reason = "SpawnPiece";
                    return true;
                case SerializableEvent.Type.SpawnDice:
                    _reason = "SpawnDice";
                    return true;
                case SerializableEvent.Type.UpdateFogAndSpawn:
                    _reason = "UpdateFogAndSpawn";
                    return true;
                case SerializableEvent.Type.SetBoardPieceID:
                    _reason = "SetBoardPieceID";
                    return true;
                case SerializableEvent.Type.NewPlayerJoin:
                    _reason = "NewPlayerJoin";
                    return true;
                case SerializableEvent.Type.Interact:
                    _reason = "Interact";
                    return true;
                case SerializableEvent.Type.Pickup:
                    _reason = "Pickup";
                    return true;
                case SerializableEvent.Type.SlimeFusion:
                    _reason = "SlimeFusion";
                    return true;
                case SerializableEvent.Type.OnMoved:
                    if (!_gameContext.pieceAndTurnController.IsPlayersTurn())
                    {
                        _reason = "OnMoved";
                        return true;
                    }

                    return false;
                case SerializableEvent.Type.Move:
                    if (_gameContext.pieceAndTurnController.IsPlayersTurn())
                    {
                        _reason = "Moved";
                        return true;
                    }

                    return false;
                case SerializableEvent.Type.OnAbilityUsed:
                    if (CanRepresentNewSpawn((SerializableEventOnAbilityUsed)serializableEvent))
                    {
                        _reason = "OnAbilityUsed";
                        return true;
                    }

                    return false;
                case SerializableEvent.Type.PieceDied:
                    if (CanRepresentNewSpawn((SerializableEventPieceDied)serializableEvent))
                    {
                        _reason = "PieceDied";
                        return true;
                    }

                    return false;
                default:
                    return false;
            }
        }

        private static bool CanRepresentNewSpawn(SerializableEventOnAbilityUsed onAbilityUsedEvent)
        {
            var abilityKey = Traverse.Create(onAbilityUsedEvent).Field<AbilityKey>("abilityKey").Value;
            switch (abilityKey)
            {
                case AbilityKey.BeastWhisperer:
                case AbilityKey.HurricaneAnthem:
                case AbilityKey.Lure:
                case AbilityKey.BoobyTrap:
                case AbilityKey.DetectEnemies:
                case AbilityKey.RepeatingBallista:
                case AbilityKey.TheBehemoth:
                case AbilityKey.HealingWard:
                case AbilityKey.RaiseRoots:
                case AbilityKey.CallCompanion:
                case AbilityKey.DigRatsNest:
                case AbilityKey.Barricade:
                case AbilityKey.MagicBarrier:
                    return true;
            }

            var abilityName = abilityKey.ToString();
            var isSpawnAbility = abilityName.Contains("Spawn");
            var isLampAbility = abilityName.Contains("Lamp");
            var isSummonAbility = abilityName.Contains("Summon");

            return isSpawnAbility || isLampAbility || isSummonAbility;
        }

        private static bool CanRepresentNewSpawn(SerializableEventPieceDied pieceDiedEvent)
        {
            foreach (var pieceId in pieceDiedEvent.deadPieces)
            {
                if (!_gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
                {
                    continue;
                }

                if (piece.boardPieceId == BoardPieceId.SpiderEgg)
                {
                    _reason = "SpiderEgg died";
                    return true;
                }
            }

            return false;
        }

        private static bool IsSyncOpportunity(SerializableEvent serializableEvent)
        {
            if (_gameContext.pieceAndTurnController.GetCurrentIndexFromTurnQueue() >= 0 && !_gameContext.pieceAndTurnController.IsPlayersTurn())
            {
                if (serializableEvent.type == SerializableEvent.Type.EndTurn)
                {
                    _reason2 = "EndTurn";
                    return true;
                }
            }

            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.EndAction:
                    _reason2 = "EndAction";
                    return _gameContext.pieceAndTurnController.IsPlayersTurn();
                case SerializableEvent.Type.CheckReopenCharacterSelect:
                    _reason2 = "CheckReopenCharacterSelect";
                    return true;
                case SerializableEvent.Type.UpdateGameHub:
                    _reason2 = "UpdateGameHub";
                    return true;
            }

            return false;
        }

        private static void SyncBoard()
        {
            // MelonLoader.MelonLogger.Warning($"Sync: [{_reason}] because [{_reason2}]");
            _reason = null;
            _reason2 = null;
            _isSyncScheduled = false;
            _gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
        }
    }
}
