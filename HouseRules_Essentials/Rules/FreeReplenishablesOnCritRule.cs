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

            if (diceResult == Dice.Outcome.Hit)
            {
                if (source.IsPlayer() && _globalAdjustments.Contains(source.boardPieceId))
                {
                    source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                    if (source.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        int currentST = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Stealthed);
                        if (currentST > 0)
                        {
                            source.effectSink.RemoveStatusEffect(EffectStateType.Stealthed);
                            source.inventory.RestoreReplenishables(source);
                            source.effectSink.AddStatusEffect(EffectStateType.Stealthed, currentST);
                            source.EnableEffectState(EffectStateType.Stealthed);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.Stealthed, currentST);
                        }
                        else
                        {
                            source.inventory.RestoreReplenishables(source);
                        }
                    }
                    else if (source.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        if (currentAP > 0)
                        {
                            AbilityFactory.TryGetAbility(AbilityKey.Zap, out var abilityZ);
                            source.inventory.RemoveDisableCooldownFlags();
                            abilityZ.effectsPreventingUse.Remove(EffectStateType.Discharge);
                            abilityZ.effectsPreventingReplenished.Remove(EffectStateType.Discharge);
                            source.inventory.RestoreReplenishables(source);
                            source.inventory.AddGold(10);
                        }
                        else
                        {
                            int money = Random.Range(11, 21);
                            source.inventory.AddGold(money);
                            source.inventory.RestoreReplenishables(source);
                        }
                    }
                    else if (currentAP > 0 || source.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        source.inventory.AddGold(10);
                        source.inventory.RestoreReplenishables(source);
                    }
                    else
                    {
                        int money = Random.Range(11, 21);
                        source.inventory.AddGold(money);
                        source.inventory.RestoreReplenishables(source);
                    }

                    HR.ScheduleBoardSync();
                }
            }

            return;
        }
    }
}
