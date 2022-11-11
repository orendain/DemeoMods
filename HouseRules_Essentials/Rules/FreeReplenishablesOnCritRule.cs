namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.BoardEntities.AI;
    using Boardgame.Cards;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

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

            if (!source.IsPlayer())
            {
                return;
            }

            // Change Frost Arrow back into Fire Arrow if used
            if (source.boardPieceId == BoardPieceId.HeroHunter)
            {
                if (source.inventory.HasAbility(AbilityKey.EnemyFrostball))
                {
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        Inventory.Item value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFrostball && value.IsReplenishing)
                        {
                            source.inventory.Items.Remove(value);
                            source.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.EnemyFireball,
                                flags = 1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                            source.inventory.ExhaustReplenishableItem(i);
                            source.AddGold(0);
                            break;
                        }
                    }
                }
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

            if (source.boardPieceId == BoardPieceId.HeroRogue)
            {
                int currentST = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Stealthed);
                if (currentST > 0)
                {
                    source.effectSink.RemoveStatusEffect(EffectStateType.Stealthed);
                    source.RestoreReplenishableAbilities();
                    source.RestoreReplenishableAbilities();
                    source.effectSink.AddStatusEffect(EffectStateType.Stealthed, currentST);
                    source.EnableEffectState(EffectStateType.Stealthed);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Stealthed, currentST);
                    source.AddGold(0);
                    return;
                }
                else
                {
                    source.RestoreReplenishableAbilities();
                    source.RestoreReplenishableAbilities();
                    return;
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroSorcerer)
            {
                if (currentAP > 0)
                {
                    if (!source.effectSink.HasEffectState(EffectStateType.Overcharge))
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Zap, out var abilityZ);
                        source.effectSink.RemoveStatusEffect(EffectStateType.Discharge);
                        abilityZ.effectsPreventingUse.Clear();
                        source.inventory.RemoveDisableCooldownFlags();

                        if (source.inventory.HasAbility(AbilityKey.Electricity, includeIsReplenishing: false, includeIsDisabled: false))
                        {
                            source.RestoreReplenishableAbilities();
                            return;
                        }
                        else
                        {
                            source.RestoreReplenishableAbilities();
                            for (int i = 0; i < source.inventory.Items.Count; i++)
                            {
                                if (source.inventory.Items[i].abilityKey == AbilityKey.Electricity)
                                {
                                    source.inventory.ExhaustReplenishableItem(i);
                                    source.AddGold(0);
                                    break;
                                }
                            }

                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroHunter)
            {
                if (currentAP > 0)
                {
                    if (source.inventory.HasAbility(AbilityKey.EnemyFireball, includeIsReplenishing: false, includeIsDisabled: false))
                    {
                        source.RestoreReplenishableAbilities();
                        return;
                    }
                    else
                    {
                        source.RestoreReplenishableAbilities();
                        for (int i = 0; i < source.inventory.Items.Count; i++)
                        {
                            if (source.inventory.Items[i].abilityKey == AbilityKey.EnemyFireball)
                            {
                                source.inventory.ExhaustReplenishableItem(i);
                                source.AddGold(0);
                                break;
                            }
                        }

                        return;
                    }
                }
                else
                {
                    source.RestoreReplenishableAbilities();
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        Inventory.Item value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFireball)
                        {
                            source.inventory.Items.Remove(value);
                            source.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.EnemyFrostball,
                                flags = 1,
                                originalOwner = -1,
                                replenishCooldown = 6,
                            });
                            source.EnableEffectState(EffectStateType.FireImmunity);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 6);
                            source.AddGold(0);
                            break;
                        }
                    }

                    return;
                }
            }

            source.RestoreReplenishableAbilities();
        }
    }
}
