namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.BoardEntities.AI;
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

            /*if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }*/

            if (!_globalAdjustments.Contains(source.boardPieceId))
            {
                return;
            }

            Inventory.Item value;
            if (source.boardPieceId == BoardPieceId.HeroRogue)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.Sneak)
                    {
                        if (value.IsReplenishing)
                        {
                            value.flags &= (Inventory.ItemFlag)(-3);
                            source.inventory.Items[i] = value;
                            source.AddGold(0);
                        }

                        break;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroBarbarian)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.Net)
                    {
                        if (value.IsReplenishing)
                        {
                            if (value.replenishCooldown < 0)
                            {
                                value.replenishCooldown = 3;
                                source.inventory.Items[i] = value;
                            }

                            value.replenishCooldown -= 1;
                            if (value.replenishCooldown < 1)
                            {
                                value.flags &= (Inventory.ItemFlag)(-3);
                            }

                            source.inventory.Items[i] = value;
                            source.AddGold(0);
                        }

                        break;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroBard)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.CourageShanty)
                    {
                        if (value.IsReplenishing)
                        {
                            value.flags &= (Inventory.ItemFlag)(-3);
                            source.inventory.Items[i] = value;
                            source.AddGold(0);
                        }

                        break;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroWarlock)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.MinionCharge)
                    {
                        if (value.IsReplenishing)
                        {
                            value.flags &= (Inventory.ItemFlag)(-3);
                            source.inventory.Items[i] = value;
                            source.AddGold(0);
                        }

                        break;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroSorcerer)
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
                        if (value.abilityKey == AbilityKey.Zap)
                        {
                            if (value.IsReplenishing)
                            {
                                value.flags &= (Inventory.ItemFlag)(-3);
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }

                            break;
                        }
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroGuardian)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.Grab)
                    {
                        if (value.IsReplenishing)
                        {
                            value.flags = (Inventory.ItemFlag)(-3);
                            source.inventory.Items[i] = value;
                            source.AddGold(0);
                        }

                        break;
                    }
                }
            }
            else if (source.boardPieceId == BoardPieceId.HeroHunter)
            {
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.Arrow)
                    {
                        if (value.IsReplenishing)
                        {
                            value.flags &= (Inventory.ItemFlag)(-3);
                            source.inventory.Items[i] = value;
                            source.AddGold(0);
                        }

                        break;
                    }
                }
            }
        }
    }
}
