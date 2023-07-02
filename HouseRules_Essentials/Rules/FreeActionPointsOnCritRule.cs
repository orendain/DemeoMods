namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class FreeActionPointsOnCritRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Some Heroes can restore Action Points by getting critical hits";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public FreeActionPointsOnCritRule(List<BoardPieceId> adjustments)
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
                    typeof(FreeActionPointsOnCritRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }

            if (!_globalAdjustments.Contains(source.boardPieceId))
            {
                return;
            }

            source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
            if (source.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                if (source.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    source.effectSink.TryGetStat(Stats.Type.Armor, out int myArmor);
                    if (myArmor < 5)
                    {
                        source.effectSink.TrySetStatBaseValue(Stats.Type.Armor, myArmor + 1);
                    }

                    if (!HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && source.GetStatMax(Stats.Type.CritChance) > 2))
                    {
                        if (currentAP < 1)
                        {
                            source.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 2);
                        }
                        else
                        {
                            source.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                        }
                    }
                    else if (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && source.GetStatMax(Stats.Type.CritChance) < 3)
                    {
                        if (currentAP < 1)
                        {
                            source.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (source.HasEffectState(EffectStateType.PlayerBerserk))
                    {
                        return;
                    }

                    source.effectSink.TryGetStat(Stats.Type.MoveRange, out int myMoveRange);
                    source.effectSink.TryGetStatMax(Stats.Type.MoveRange, out int myMaxMoveRange);
                    source.effectSink.TrySetStatMaxValue(Stats.Type.MoveRange, myMaxMoveRange + 3);
                    source.effectSink.TrySetStatBaseValue(Stats.Type.MoveRange, myMoveRange + 3);
                    source.EnableEffectState(EffectStateType.PlayerBerserk);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.PlayerBerserk, 1);
                }
                else if (source.boardPieceId == BoardPieceId.HeroRogue)
                {
                    if (currentAP < 1)
                    {
                        source.EnableEffectState(EffectStateType.Invisibility);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.Invisibility, 2);
                    }
                    else if (!HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && source.GetStatMax(Stats.Type.CritChance) > 2))
                    {
                        source.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                    }
                }
            }
            else
            {
                source.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
            }
        }
    }
}
