namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardEnergyAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card additions from mana are overridden";

        private static Dictionary<BoardPieceId, List<AbilityKey>> _globalenergyCards;
        private static bool _isActivated;
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

            if (HR.SelectedRuleset.Name.Contains("PROGRESSIVE"))
            {
                int nextLevel = piece.GetStat(Stats.Type.BonusCorruptionDamage);
                if (nextLevel < 10)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.BonusCorruptionDamage, nextLevel + 1);
                    nextLevel++;
                    piece.effectSink.Heal(2);
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Downed);
                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal, 1);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.StrengthInNumbers, piece.GetStat(Stats.Type.BonusCorruptionDamage));
                    if (nextLevel == 3 || nextLevel == 6)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 1);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 1);
                    }
                    else if (nextLevel == 9)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);
                    }
                    else if (nextLevel == 4 || nextLevel == 8)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);
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
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                        {
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                            piece.inventory.Items.Add(new Inventory.Item
                            {
                                abilityKey = AbilityKey.Electricity,
                                flags = (Inventory.ItemFlag)1,
                                originalOwner = -1,
                                replenishCooldown = 1,
                            });
                            piece.AddGold(0);
                        }
                        else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);
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
            if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
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
