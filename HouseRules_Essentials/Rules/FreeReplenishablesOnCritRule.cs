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

            if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }

            if (!_globalAdjustments.Contains(source.boardPieceId))
            {
                return;
            }

            Inventory.Item value;
            source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);

            if (source.boardPieceId == BoardPieceId.HeroRogue)
            {
                int currentST = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Stealthed);
                if (currentST > 0)
                {
                    source.effectSink.RemoveStatusEffect(EffectStateType.Stealthed);
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.DiseasedBite)
                        {
                            if (value.IsReplenishing)
                            {
                                if (value.replenishCooldown < 0)
                                {
                                    value.replenishCooldown = 2;
                                }
                                else
                                {
                                    value.replenishCooldown = 1;
                                }

                                source.inventory.Items[i] = value;
                            }
                        }
                        else if (value.abilityKey == AbilityKey.FretsOfFire)
                        {
                            if (value.IsReplenishing)
                            {
                                value.replenishCooldown = 1;
                                source.inventory.Items[i] = value;
                            }
                        }
                    }

                    source.RestoreReplenishableAbilities();
                    source.RestoreReplenishableAbilities();
                    source.effectSink.AddStatusEffect(EffectStateType.Stealthed, currentST);
                    source.EnableEffectState(EffectStateType.Stealthed);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Stealthed, currentST);
                    return;
                }
                else
                {
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.DiseasedBite)
                        {
                            if (value.IsReplenishing)
                            {
                                if (value.replenishCooldown < 0)
                                {
                                    value.replenishCooldown = 2;
                                }
                                else
                                {
                                    value.replenishCooldown = 1;
                                }

                                source.inventory.Items[i] = value;
                            }
                        }
                        else if (value.abilityKey == AbilityKey.FretsOfFire)
                        {
                            if (value.IsReplenishing)
                            {
                                value.replenishCooldown = 1;
                                source.inventory.Items[i] = value;
                            }
                        }
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroBard)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.EnemyFlashbang)
                    {
                        if (value.IsReplenishing)
                        {
                            if (value.replenishCooldown < 0)
                            {
                                value.replenishCooldown = 2;
                            }
                            else
                            {
                                value.replenishCooldown = 1;
                            }

                            source.inventory.Items[i] = value;
                        }
                    }
                    else if (value.abilityKey == AbilityKey.PVPBlink)
                    {
                        if (value.IsReplenishing)
                        {
                            value.replenishCooldown = 1;
                            source.inventory.Items[i] = value;
                        }
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroWarlock)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.MagicMissile)
                    {
                        if (value.IsReplenishing)
                        {
                            value.replenishCooldown = 1;
                            source.inventory.Items[i] = value;
                        }
                    }
                    else if (value.abilityKey == AbilityKey.Weaken)
                    {
                        if (value.IsReplenishing)
                        {
                            value.replenishCooldown = 1;
                            source.inventory.Items[i] = value;
                        }
                    }
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

                        for (int i = 0; i < source.inventory.Items.Count; i++)
                        {
                            value = source.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.Electricity)
                            {
                                if (value.IsReplenishing)
                                {
                                    value.replenishCooldown = 1;
                                    source.inventory.Items[i] = value;
                                }
                            }
                            else if (value.abilityKey == AbilityKey.SpellPowerPotion)
                            {
                                if (value.IsReplenishing)
                                {
                                    value.replenishCooldown = 1;
                                    source.inventory.Items[i] = value;
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroGuardian)
            {
                if (currentAP > 0)
                {
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.LeapHeavy)
                        {
                            if (value.IsReplenishing)
                            {
                                value.replenishCooldown = 1;
                                source.inventory.Items[i] = value;
                            }
                        }
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroHunter)
            {
                if (currentAP > 0)
                {
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFireball)
                        {
                            if (value.IsReplenishing)
                            {
                                value.replenishCooldown = 1;
                                source.inventory.Items[i] = value;
                            }
                        }
                        else if (value.abilityKey == AbilityKey.SpawnRandomLamp)
                        {
                            if (value.IsReplenishing)
                            {
                                value.replenishCooldown = 1;
                                source.inventory.Items[i] = value;
                            }
                        }
                    }
                }
                else
                {
                    source.RestoreReplenishableAbilities();
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFireball)
                        {
                            source.inventory.Items.Remove(value);
                            source.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.EnemyFrostball,
                                flags = 1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                        }

                        source.EnableEffectState(EffectStateType.FireImmunity);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 6);
                        source.AddGold(0);
                        break;
                    }

                    return;
                }
            }

            source.RestoreReplenishableAbilities();
        }
    }
}
