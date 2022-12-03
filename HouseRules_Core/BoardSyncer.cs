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
        private static bool _isMove;
        private static bool _isStateChange;
        private static bool _onMoved;
        private static bool _ignoreOnMoved;

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

        private static void Piece_IsImmuneToStatusEffect_Postfix()
        {
            var isEffectImmunityCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.StatusEffectImmunityModified) > 0;
            if (isEffectImmunityCheckRequired)
            {
                // CoreMod.Logger.Msg("(StateChange) Immunity");
                _isStateChange = true;
                _isSyncScheduled = true;
            }
        }

        private static void EffectSink_AddStatusEffect_Postfix()
        {
            var isEffectDataCheckRequired = (HR.SelectedRuleset.ModifiedSyncables & SyncableTrigger.StatusEffectDataModified) > 0;
            if (isEffectDataCheckRequired)
            {
                // CoreMod.Logger.Msg("(StateChange) Effect");
                _isStateChange = true;
                _isSyncScheduled = true;
            }
        }

        private static bool CanRepresentNewSpawn(SerializableEvent serializableEvent)
        {
            string whatUp = serializableEvent.ToString();
            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.OnMoved:
                    var pieceId = Traverse.Create(serializableEvent).Field<int>("pieceId").Value;
                    if (pieceId > 0 && _gameContext.pieceAndTurnController.IsPlayerControlled(pieceId))
                    {
                        // CoreMod.Logger.Msg("(OnMoved) Player");
                        _onMoved = true;
                        return true;
                    }

                    // CoreMod.Logger.Msg($"------ {whatUp}");
                    return false;
                case SerializableEvent.Type.Move:
                case SerializableEvent.Type.Interact:
                    if (_gameContext.pieceAndTurnController.IsPlayersTurn())
                    {
                        // CoreMod.Logger.Msg("(Move) Player");
                        // CoreMod.Logger.Msg($"<<<>>> {whatUp}");
                        _isMove = true;
                        return true;
                    }

                    // CoreMod.Logger.Msg($"------ {whatUp}");
                    return false;
                case SerializableEvent.Type.NewPlayerJoin:
                case SerializableEvent.Type.SpawnPiece:
                case SerializableEvent.Type.SetBoardPieceID:
                case SerializableEvent.Type.SlimeFusion:
                    // CoreMod.Logger.Msg("(BoardPiece) New Piece/Player");
                    // CoreMod.Logger.Msg($"<<<>>> {whatUp}");
                    return true;
                case SerializableEvent.Type.OnAbilityUsed:
                    return CanRepresentNewSpawn((SerializableEventOnAbilityUsed)serializableEvent);
                case SerializableEvent.Type.PieceDied:
                    return CanRepresentNewSpawn((SerializableEventPieceDied)serializableEvent);
                default:
                    // CoreMod.Logger.Msg($"------ {whatUp}");
                    return false;
            }
        }

        private static bool CanRepresentNewSpawn(SerializableEventOnAbilityUsed onAbilityUsedEvent)
        {
            var abilityKey = Traverse.Create(onAbilityUsedEvent).Field<AbilityKey>("abilityKey").Value;
            switch (abilityKey)
            {
                case AbilityKey.Grab:
                case AbilityKey.WhirlwindAttack:
                    if (_gameContext.pieceAndTurnController.IsPlayersTurn())
                    {
                        // CoreMod.Logger.Msg("(Whirlwind) Player");
                        _ignoreOnMoved = true;
                        return true;
                    }

                    // CoreMod.Logger.Msg("(Whirlwind) Enemy");
                    _onMoved = true;
                    return true;
                case AbilityKey.Tsunami:
                case AbilityKey.ScrollTsunami:
                case AbilityKey.AcidSpit:
                case AbilityKey.Corrupt:
                case AbilityKey.CorruptOneTurn:
                case AbilityKey.DiseasedBite:
                case AbilityKey.GasLamp:
                case AbilityKey.PoisonGas:
                case AbilityKey.PoisonBomb:
                case AbilityKey.Petrify:
                case AbilityKey.Freeze:
                case AbilityKey.IceExplosion:
                case AbilityKey.LightningBolt:
                case AbilityKey.Zap:
                case AbilityKey.RevealPath:
                case AbilityKey.DetectEnemies:
                case AbilityKey.BeastWhisperer:
                case AbilityKey.HurricaneAnthem:
                case AbilityKey.Lure:
                case AbilityKey.BoobyTrap:
                case AbilityKey.RepeatingBallista:
                case AbilityKey.TheBehemoth:
                case AbilityKey.HealingWard:
                case AbilityKey.RaiseRoots:
                case AbilityKey.CallCompanion:
                case AbilityKey.DigRatsNest:
                case AbilityKey.Barricade:
                case AbilityKey.MagicBarrier:
                    // CoreMod.Logger.Msg("(Ability) Spawn/Effect");
                    return true;
            }

            var abilityName = abilityKey.ToString();
            var isSpawnAbility = abilityName.Contains("Spawn");
            var isLampAbility = abilityName.Contains("Lamp");
            var isSummonAbility = abilityName.Contains("Summon");

            if (isSpawnAbility || isLampAbility || isSummonAbility)
            {
                // CoreMod.Logger.Msg("(Summon) Creature/Lamp");
                return true;
            }

            return false;
        }

        private static bool CanRepresentNewSpawn(SerializableEventPieceDied pieceDiedEvent)
        {
            foreach (var pieceId in pieceDiedEvent.deadPieces)
            {
                if (!_gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
                {
                    continue;
                }

                if (piece.boardPieceId == BoardPieceId.SpiderEgg || piece.boardPieceId == BoardPieceId.ScorpionSandPile)
                {
                    // CoreMod.Logger.Msg("(Died) SpiderEgg/SandPile");
                    return true;
                }
            }

            return false;
        }

        private static bool IsSyncOpportunity(SerializableEvent serializableEvent)
        {
            string whatUp = serializableEvent.ToString();
            // CoreMod.Logger.Msg($"=SYNC= {whatUp}");

            if (_ignoreOnMoved)
            {
                if (serializableEvent.type == SerializableEvent.Type.EndAction)
                {
                    // CoreMod.Logger.Msg("(Whirlwind) EndAction");
                    _ignoreOnMoved = false;
                    if (_isMove || _isStateChange)
                    {
                        // CoreMod.Logger.Msg("(Whirlwind) StateChange");
                        return true;
                    }

                    _isSyncScheduled = false;
                    return false;
                }

                return false;
            }
            else if (_onMoved && serializableEvent.type == SerializableEvent.Type.OnMoved)
            {
                // CoreMod.Logger.Msg("(OnMoved) Player");
                _onMoved = false;
                _isSyncScheduled = false;
                _gameContext.serializableEventQueue.SendResponseEvent(new SerializableEventUpdateFog());
                return false;
            }

            if ((_isMove || _isStateChange) && (serializableEvent.type == SerializableEvent.Type.EndAction || serializableEvent.type == SerializableEvent.Type.EndTurn))
            {
                // CoreMod.Logger.Msg("(Move/StateChange) EndAction/EndTurn");
                return true;
            }

            if (_gameContext.pieceAndTurnController.GetCurrentIndexFromTurnQueue() >= 0 && !_gameContext.pieceAndTurnController.IsPlayersTurn())
            {
                if (serializableEvent.type == SerializableEvent.Type.EndTurn)
                {
                    // CoreMod.Logger.Msg("(Enemies) EndTurn");
                    return true;
                }

                return false;
            }

            if (!_isMove && !_isStateChange && !_onMoved)
            {
                // CoreMod.Logger.Msg("(Default Recovery) Not Move/OnMoved/StateChange");
                return true;
            }

            return false;
        }

        private static void SyncBoard()
        {
            // CoreMod.Logger.Msg("<<< !!RECOVERY!! >>>");
            _isMove = false;
            _isStateChange = false;
            _isSyncScheduled = false;
            _gameContext.serializableEventQueue.SendResponseEvent(new SerializableEventUpdateFogAndSpawn());
            _gameContext.serializableEventQueue.SendResponseEvent(SerializableEvent.CreateRecovery());
            _gameContext.serializableEventQueue.SendResponseEvent(new SerializableEventUpdateFogAndSpawn());
        }
    }
}
