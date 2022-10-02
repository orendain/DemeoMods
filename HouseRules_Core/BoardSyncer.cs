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

            /*harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "IsImmuneToStatusEffect"),
                postfix: new HarmonyMethod(
                    typeof(BoardSyncer),
                    nameof(Piece_IsImmuneToStatusEffect_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(EffectSink), "AddStatusEffect"),
                postfix: new HarmonyMethod(
                    typeof(BoardSyncer),
                    nameof(EffectSink_AddStatusEffect_Postfix)));*/

            // Workaround for a bug in Demeo v1.21 affecting board syncing. Can be removed after next Demeo patch is released as it includes fix from RG.
            /*harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "ForceSyncState"),
                prefix: new HarmonyMethod(
                    typeof(BoardSyncer),
                    nameof(Piece_ForceSyncState_Prefix)));*/
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

            var isNewPieceCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.NewPieceModified) > 0;
            if (!_isSyncScheduled && isNewPieceCheckRequired && CanRepresentNewSpawn(serializableEvent))
            {
                _isSyncScheduled = true;
            }

            if (_isSyncScheduled && IsSyncOpportunity(serializableEvent))
            {
                SyncBoard();
            }
        }

        /*private static void Piece_IsImmuneToStatusEffect_Postfix()
        {
            var isEffectImmunityCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.StatusEffectImmunityModified) > 0;
            if (isEffectImmunityCheckRequired)
            {
                //MelonLoader.MelonLogger.Msg("ImmunityToStatusEffect");
                //_isSyncScheduled = true;
            }
        }

        private static void EffectSink_AddStatusEffect_Postfix()
        {
            var isEffectDataCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.StatusEffectDataModified) > 0;
            if (isEffectDataCheckRequired)
            {
                //MelonLoader.MelonLogger.Msg("StatusEffect");
                //_isSyncScheduled = true;
            }
        }*/

        /*private static bool Piece_ForceSyncState_Prefix(BoardModel boardModel, ref Piece __instance)
        {
            __instance.ReregisterPieceVisualStateHandlers();
            return true;
        }*/

        private static bool CanRepresentNewSpawn(SerializableEvent serializableEvent)
        {
            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.UpdateFogAndSpawn:
                    MelonLoader.MelonLogger.Msg("UpdateFogAndSpawn");
                    return true;
                /*case SerializableEvent.Type.SpawnPiece:
                    MelonLoader.MelonLogger.Msg("SpawnPiece");
                    return true;
                case SerializableEvent.Type.SetBoardPieceID:
                    MelonLoader.MelonLogger.Msg("SetBoardPieceID");
                    return true;
                case SerializableEvent.Type.SlimeFusion:
                    MelonLoader.MelonLogger.Msg("SlimeFusion");
                    return true;*/
                case SerializableEvent.Type.OnAbilityUsed:
                    return CanRepresentNewSpawn((SerializableEventOnAbilityUsed)serializableEvent);
                /*case SerializableEvent.Type.PieceDied:
                    return CanRepresentNewSpawn((SerializableEventPieceDied)serializableEvent);*/
                default:
                    return false;
            }
        }

        private static bool CanRepresentNewSpawn(SerializableEventOnAbilityUsed onAbilityUsedEvent)
        {
            var abilityKey = Traverse.Create(onAbilityUsedEvent).Field<AbilityKey>("abilityKey").Value;
            switch (abilityKey)
            {
                case AbilityKey.SummonElemental:
                    MelonLoader.MelonLogger.Msg("SummonElemental");
                    return true;
                case AbilityKey.SummonBossMinions:
                    MelonLoader.MelonLogger.Msg("SummonBossMinions");
                    return true;
                case AbilityKey.BeastWhisperer:
                    MelonLoader.MelonLogger.Msg("BeastWhisperer");
                    return true;
                case AbilityKey.HurricaneAnthem:
                    MelonLoader.MelonLogger.Msg("HurricaneAnthem");
                    return true;
                case AbilityKey.Lure:
                    MelonLoader.MelonLogger.Msg("Lure");
                    return true;
                case AbilityKey.BoobyTrap:
                    MelonLoader.MelonLogger.Msg("BoobyTrap");
                    return true;
                case AbilityKey.DetectEnemies:
                    MelonLoader.MelonLogger.Msg("DetectEnemies");
                    return true;
                case AbilityKey.RepeatingBallista:
                    MelonLoader.MelonLogger.Msg("RepeatingBallista");
                    return true;
                case AbilityKey.TheBehemoth:
                    MelonLoader.MelonLogger.Msg("TheBehemoth");
                    return true;
                case AbilityKey.HealingWard:
                    MelonLoader.MelonLogger.Msg("HealingWard");
                    return true;
                case AbilityKey.RaiseRoots:
                    MelonLoader.MelonLogger.Msg("RaiseRoots");
                    return true;
                case AbilityKey.CallCompanion:
                    MelonLoader.MelonLogger.Msg("CallCompanion");
                    return true;
                case AbilityKey.DigRatsNest:
                    MelonLoader.MelonLogger.Msg("DigRatsNest");
                    return true;
                case AbilityKey.Barricade:
                    MelonLoader.MelonLogger.Msg("Barricade");
                    return true;
                case AbilityKey.MagicBarrier:
                    MelonLoader.MelonLogger.Msg("MagicBarrier");
                    return true;
            }

            var abilityName = abilityKey.ToString();
            var isSpawnAbility = abilityName.Contains("Spawn");
            var isLampAbility = abilityName.Contains("Lamp");
            MelonLoader.MelonLogger.Msg($"{isSpawnAbility} or {isLampAbility}");

            //return isSpawnAbility || isLampAbility;
            return false;
        }

        /*private static bool CanRepresentNewSpawn(SerializableEventPieceDied pieceDiedEvent)
        {
            foreach (var pieceId in pieceDiedEvent.deadPieces)
            {
                if (!_gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
                {
                    continue;
                }

                if (piece.boardPieceId == BoardPieceId.SpiderEgg)
                {
                    MelonLoader.MelonLogger.Msg("SpiderEgg");
                    return true;
                }
            }

            return false;
        }*/

        private static bool IsSyncOpportunity(SerializableEvent serializableEvent)
        {
            if (_gameContext.pieceAndTurnController.GetCurrentIndexFromTurnQueue() >= 0 && !_gameContext.pieceAndTurnController.IsPlayersTurn())
            {
                return serializableEvent.type == SerializableEvent.Type.EndTurn;
            }

            return true;
        }

        private static void SyncBoard()
        {
            _isSyncScheduled = false;
            MelonLoader.MelonLogger.Msg("Board Sync!");
            //_gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
        }
    }
}
