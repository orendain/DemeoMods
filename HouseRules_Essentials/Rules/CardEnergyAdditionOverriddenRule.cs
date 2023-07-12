namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardEnergyAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card loot from energy (mana) is adjusted";

        private static Dictionary<BoardPieceId, List<AbilityKey>> _globalenergyCards;
        private static bool _isActivated;
        private static bool _dropchest;
        private static int _numEnergy;
        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _energyCards;

        public CardEnergyAdditionOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> energyCards)
        {
            _energyCards = energyCards;
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _energyCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalenergyCards = _energyCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _numEnergy = 0;
            _dropchest = false;
            _isActivated = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                prefix: new HarmonyMethod(
                    typeof(CardEnergyAdditionOverriddenRule),
                    nameof(SerializableEventQueue_RespondToRequest_Prefix)));
        }

        public static class RandomProvider
        {
            private static int seed = Environment.TickCount;

            private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(
                () => new Random(Interlocked.Increment(ref seed)));

            public static Random GetThreadRandom()
            {
                return randomWrapper.Value;
            }
        }

        private static void SerializableEventQueue_RespondToRequest_Prefix(
            SerializableEventQueue __instance,
            ref SerializableEvent request)
        {
            if (!_isActivated)
            {
                return;
            }

            if (request.type != SerializableEvent.Type.AddCardToPiece)
            {
                return;
            }

            var addCardToPieceEvent = (SerializableEventAddCardToPiece)request;
            var gameContext = Traverse.Create(__instance).Property<GameContext>("gameContext").Value;
            var pieceId = Traverse.Create(addCardToPieceEvent).Field<int>("pieceId").Value;
            var cardSource = Traverse.Create(addCardToPieceEvent).Field<int>("cardSource").Value;

            if (cardSource != (int)MotherTracker.Context.Energy)
            {
                return;
            }

            if (!gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            if (!_globalenergyCards.TryGetValue(piece.boardPieceId, out var replacementAbilityKeys))
            {
                return;
            }

            if (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE"))
            {
                int nextLevel = piece.GetStatMax(Stats.Type.CritChance);
                if (nextLevel < 10)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, nextLevel + 1);
                    nextLevel++;
                    piece.effectSink.Heal(2);
                    if (piece.HasEffectState(EffectStateType.Downed))
                    {
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Downed);
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Stunned);
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Frozen);
                    }

                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal, 1);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, piece.GetStatMax(Stats.Type.CritChance));
                    if (nextLevel == 3)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 1);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 1);
                    }
                    else if (nextLevel == 6)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);
                    }
                    else if (nextLevel == 9)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 3);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 3);
                    }
                    else if (nextLevel == 4)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);
                    }
                    else if (nextLevel == 8)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);
                        int randAbil = RandomProvider.GetThreadRandom().Next(5);
                        if (randAbil == 4 && _dropchest)
                        {
                            randAbil = RandomProvider.GetThreadRandom().Next(4);
                        }

                        if (randAbil == 0)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.Petrify,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 7,
                            });
                        }
                        else if (randAbil == 1)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.AcidSpit,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 7,
                            });
                        }
                        else if (randAbil == 2)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.DeathFlurry,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 7,
                            });
                        }
                        else if (randAbil == 3)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.Shockwave,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 7,
                            });
                        }
                        else if (randAbil == 4)
                        {
                            _dropchest = true;
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.DropChest,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 7,
                            });
                        }

                        piece.AddGold(0);
                    }
                    else if (nextLevel == 2)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.Net,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 3,
                            });
                            piece.AddGold(0);

                            AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var ability);
                            ability.costActionPoint = false;
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroBard)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.EnemyFlashbang,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 3,
                            });
                            piece.AddGold(0);

                            AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                            ability.costActionPoint = false;
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.Grab,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                            piece.AddGold(0);
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.DiseasedBite,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 3,
                            });
                            piece.AddGold(0);

                            AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                            ability.costActionPoint = false;
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                        {
                            Inventory.Item value;
                            for (var i = 0; i < piece.inventory.Items.Count; i++)
                            {
                                value = piece.inventory.Items[i];
                                if (value.abilityKey == AbilityKey.Arrow)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                    break;
                                }
                            }

                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.EnemyFireball,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                            piece.AddGold(0);

                            AbilityFactory.TryGetAbility(AbilityKey.Arrow, out var ability);
                            ability.costActionPoint = false;
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                        {
                            Inventory.Item value;
                            for (var i = 0; i < piece.inventory.Items.Count; i++)
                            {
                                value = piece.inventory.Items[i];
                                if (value.abilityKey == AbilityKey.Overcharge)
                                {
                                    piece.inventory.Items.Remove(value);
                                    break;
                                }
                            }

                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.Electricity,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                            piece.AddGold(0);

                            AbilityFactory.TryGetAbility(AbilityKey.Zap, out var ability);
                            AbilityFactory.TryGetAbility(AbilityKey.LightningBolt, out var ability2);
                            ability.costActionPoint = false;
                            ability2.costActionPoint = false;
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.MinionCharge,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                            piece.AddGold(0);

                            AbilityFactory.TryGetAbility(AbilityKey.MinionCharge, out var ability);
                            ability.costActionPoint = false;
                        }
                    }
                    else if (nextLevel == 5)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);
                        }
                        else
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);
                        }
                    }
                    else if (nextLevel == 10)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 2);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 2);
                        }
                        else
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 2);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 2);
                        }

                        piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                    }
                    else if (nextLevel == 7)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) + 2);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) + 2);
                    }
                }
            }

            int rand;
            AbilityKey replacementAbilityKey;
            int randNum = RandomProvider.GetThreadRandom().Next(101);
            if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Revolutions"))
            {
                if (randNum > 90 && _numEnergy < 2)
                {
                    if (!piece.HasEffectState(EffectStateType.ExtraEnergy))
                    {
                        // Energy Potion
                        _numEnergy++;
                        rand = 0;
                    }
                    else
                    {
                        // Standard cards
                        rand = RandomProvider.GetThreadRandom().Next(1, replacementAbilityKeys.Count);
                    }
                }
                else
                {
                    // Standard cards
                    rand = RandomProvider.GetThreadRandom().Next(1, replacementAbilityKeys.Count);
                }
            }
            else
            {
                rand = RandomProvider.GetThreadRandom().Next(0, replacementAbilityKeys.Count);
            }

            replacementAbilityKey = replacementAbilityKeys[rand];
            Traverse.Create(addCardToPieceEvent).Field<AbilityKey>("card").Value = replacementAbilityKey;
        }
    }
}
