namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class FreeAbilityOnCritRule : Rule, IConfigWritable<Dictionary<BoardPieceId, AbilityKey>>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Some Heroes can get a free card by getting critical hits";

        private static Dictionary<BoardPieceId, AbilityKey> _globalAdjustments;
        private static bool _isActivated;

        private readonly Dictionary<BoardPieceId, AbilityKey> _adjustments;

        public FreeAbilityOnCritRule(Dictionary<BoardPieceId, AbilityKey> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<BoardPieceId, AbilityKey> GetConfigObject() => _adjustments;

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
                prefix: new HarmonyMethod(
                    typeof(FreeAbilityOnCritRule),
                    nameof(Ability_GenerateAttackDamage_Prefix)));
        }

        private static void Ability_GenerateAttackDamage_Prefix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            if (source.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                if (source.boardPieceId == BoardPieceId.HeroBard)
                {
                    if (!HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && (source.GetStatMax(Stats.Type.CritChance) > 2 || currentAP < 1)))
                    {
                        if (source.HasEffectState(EffectStateType.Fearless))
                        {
                            source.EnableEffectState(EffectStateType.Fearless);
                        }
                        else if (source.HasEffectState(EffectStateType.Heroic))
                        {
                            source.DisableEffectState(EffectStateType.Heroic);
                            source.EnableEffectState(EffectStateType.Fearless);
                        }
                        else if (source.HasEffectState(EffectStateType.Courageous))
                        {
                            source.DisableEffectState(EffectStateType.Courageous);
                            source.EnableEffectState(EffectStateType.Heroic);
                        }
                        else
                        {
                            source.EnableEffectState(EffectStateType.Courageous);
                        }
                    }
                }
                else if (source.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    source.effectSink.TryGetStat(Stats.Type.MagicArmor, out int myArmor);
                    if (currentAP < 1)
                    {
                        if (myArmor < 9)
                        {
                            if (!HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && source.GetStatMax(Stats.Type.CritChance) > 2))
                            {
                                source.effectSink.TrySetStatBaseValue(Stats.Type.MagicArmor, myArmor + 2);
                            }
                            else
                            {
                                source.effectSink.TrySetStatBaseValue(Stats.Type.MagicArmor, myArmor + 1);
                            }
                        }
                        else if (myArmor == 9)
                        {
                            source.effectSink.TrySetStatBaseValue(Stats.Type.MagicArmor, myArmor + 1);
                        }

                        int myVargas = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.MarkOfVerga);
                        source.EnableEffectState(EffectStateType.MarkOfVerga, myVargas + 6);
                    }
                    else
                    {
                        if (!HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && source.GetStatMax(Stats.Type.CritChance) > 2))
                        {
                            if (myArmor < 10)
                            {
                                source.effectSink.TrySetStatBaseValue(Stats.Type.MagicArmor, myArmor + 1);
                            }
                        }
                    }
                }
                else if (source.boardPieceId == BoardPieceId.HeroSorcerer && source.effectSink.HasEffectState(EffectStateType.Overcharge))
                {
                    if (!HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") && source.GetStatMax(Stats.Type.CritChance) > 2))
                    {
                        Inventory.Item value1;
                        bool hasPower1 = false;
                        for (int i = 0; i < source.inventory.Items.Count; i++)
                        {
                            value1 = source.inventory.Items[i];
                            if (value1.abilityKey == AbilityKey.WaterBottle)
                            {
                                hasPower1 = true;
                                if (value1.IsReplenishing)
                                {
                                    value1.flags &= (Inventory.ItemFlag)(-3);
                                    source.inventory.Items[i] = value1;
                                    source.AddGold(0);
                                }

                                break;
                            }
                        }

                        if (!hasPower1)
                        {
                            source.TryAddAbilityToInventory(AbilityKey.WaterBottle, showTooltip: true, isReplenishable: true);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                if (currentAP > 0)
                {
                    return;
                }

                if (source.boardPieceId == BoardPieceId.HeroBard)
                {
                    if (!source.HasEffectState(EffectStateType.DeflectionBarrier))
                    {
                        source.effectSink.TryGetStat(Stats.Type.MoveRange, out int myMoveRange);
                        source.effectSink.TryGetStatMax(Stats.Type.MoveRange, out int myMaxMoveRange);
                        source.effectSink.TrySetStatMaxValue(Stats.Type.MoveRange, myMaxMoveRange + 3);
                        source.effectSink.TrySetStatBaseValue(Stats.Type.MoveRange, myMoveRange + 3);
                        source.EnableEffectState(EffectStateType.DeflectionBarrier);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.DeflectionBarrier, 3);
                    }
                    else
                    {
                        source.effectSink.SetStatusEffectDuration(EffectStateType.DeflectionBarrier, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.DeflectionBarrier) + 3);
                    }
                }
                else if (source.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    if (!source.HasEffectState(EffectStateType.SpawnBuildUp))
                    {
                        source.effectSink.TryGetStat(Stats.Type.MagicBonus, out int myMagic);
                        source.effectSink.TryGetStatMax(Stats.Type.MagicBonus, out int myMaxMagic);
                        source.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, myMaxMagic + 3);
                        source.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, myMagic + 3);
                        source.EnableEffectState(EffectStateType.SpawnBuildUp);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.SpawnBuildUp, 2);
                    }
                    else
                    {
                        source.effectSink.SetStatusEffectDuration(EffectStateType.SpawnBuildUp, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.SpawnBuildUp) + 2);
                    }

                    if (!source.HasEffectState(EffectStateType.Deflect))
                    {
                        source.EnableEffectState(EffectStateType.Deflect);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.Deflect, 2);
                    }
                    else
                    {
                        source.effectSink.SetStatusEffectDuration(EffectStateType.Deflect, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Deflect) + 2);
                    }
                }
                else if (source.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    source.DisableEffectState(EffectStateType.Wet);
                    source.EnableEffectState(EffectStateType.Overcharge);
                }
                else if (source.boardPieceId == BoardPieceId.HeroHunter)
                {
                    Inventory.Item value;
                    bool hasPower = false;
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFrostball)
                        {
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                value.flags &= (Inventory.ItemFlag)(-3);
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }

                            break;
                        }
                    }

                    if (hasPower)
                    {
                        if (!source.HasEffectState(EffectStateType.FireImmunity))
                        {
                            source.EnableEffectState(EffectStateType.FireImmunity);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 6);
                        }
                        else
                        {
                            source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.FireImmunity) + 6);
                        }
                    }
                    else
                    {
                        Traverse.Create(source.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        source.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFrostball,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });

                        source.AddGold(0);
                        if (!source.HasEffectState(EffectStateType.FireImmunity))
                        {
                            source.EnableEffectState(EffectStateType.FireImmunity);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 6);
                        }
                        else
                        {
                            source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.FireImmunity) + 6);
                        }
                    }
                }
            }

            if (!_globalAdjustments.ContainsKey(source.boardPieceId))
            {
                return;
            }

            Inventory.Item value2;
            bool hasPower2 = false;
            for (int i = 0; i < source.inventory.Items.Count; i++)
            {
                value2 = source.inventory.Items[i];
                if (value2.abilityKey == _globalAdjustments[source.boardPieceId])
                {
                    hasPower2 = true;
                    if (value2.IsReplenishing)
                    {
                        value2.flags &= (Inventory.ItemFlag)(-3);
                        source.inventory.Items[i] = value2;
                        source.AddGold(0);
                    }

                    break;
                }
            }

            if (source.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                if (!hasPower2)
                {
                    source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], showTooltip: true, isReplenishable: true);
                }
            }
            else
            {
                source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], showTooltip: true, isReplenishable: false);
            }
        }
    }
}
