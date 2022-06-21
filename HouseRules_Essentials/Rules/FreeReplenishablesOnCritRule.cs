namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class FreeReplenishablesOnCritRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Critical Hit gives gold & restores replenishables.";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public FreeReplenishablesOnCritRule(List<BoardPieceId> adjustments)
        {
            _adjustments = adjustments;
        }

        public List<BoardPieceId> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
          }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Ability), "GenerateAttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(FreeReplenishablesOnCritRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            MelonLoader.MelonLogger.Msg("Free Replenish called");
            if (diceResult == Dice.Outcome.Crit)
            {
                MelonLoader.MelonLogger.Msg("Replenish(0)");
                if (source.IsPlayer() && _globalAdjustments.Contains(source.boardPieceId))
                {
                    MelonLoader.MelonLogger.Msg("Replenish(1)");
                    source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                    int money = Random.Range(11, 21);
                    MelonLoader.MelonLogger.Msg("Replenish(2)");
                    if (source.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        MelonLoader.MelonLogger.Msg("Replenish: Assassin check");
                        int currentST = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Stealthed);
                        if (currentST > 0)
                        {
                            MelonLoader.MelonLogger.Msg("Replenish: Stealth refresh(1)");
                            source.effectSink.RemoveStatusEffect(EffectStateType.Stealthed);
                            source.inventory.RestoreReplenishables(source);
                            source.effectSink.AddStatusEffect(EffectStateType.Stealthed, currentST);
                            source.EnableEffectState(EffectStateType.Stealthed);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.Stealthed, currentST);
                        }
                        else
                        {
                            MelonLoader.MelonLogger.Msg("Replenish: Stealth refresh(2)");
                            source.inventory.RestoreReplenishables(source);
                        }
                    }
                    else if (source.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        MelonLoader.MelonLogger.Msg("Replenish: Sorcerer check");
                        if (currentAP > 0)
                        {
                            MelonLoader.MelonLogger.Msg("Replenish: Zap refresh(1)");
                            AbilityFactory.TryGetAbility(AbilityKey.Zap, out var abilityZ);
                            source.inventory.RemoveDisableCooldownFlags();
                            abilityZ.effectsPreventingUse.Remove(EffectStateType.Discharge);
                            abilityZ.effectsPreventingReplenished.Remove(EffectStateType.Discharge);
                            source.inventory.AddGold(10);
                            source.inventory.RestoreReplenishables(source);
                        }
                        else
                        {
                            MelonLoader.MelonLogger.Msg("Replenish: Zap refresh(2)");
                            source.inventory.AddGold(money);
                        }
                    }
                    else if (currentAP > 0 || source.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        MelonLoader.MelonLogger.Msg("Replenish(3)");
                        source.inventory.AddGold(10);
                        source.inventory.RestoreReplenishables(source);
                    }
                    else
                    {
                        MelonLoader.MelonLogger.Msg("Replenish(4)");
                        source.inventory.AddGold(money);
                        source.inventory.RestoreReplenishables(source);
                    }

                    MelonLoader.MelonLogger.Msg("Replenish(5)");
                    HR.ScheduleBoardSync();
                }
            }

            MelonLoader.MelonLogger.Msg("Replenish FINISHED!");
            return;
        }
    }
}
