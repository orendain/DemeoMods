namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Boardgame;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardEnergyAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card additions from mana are overridden";

        private static Dictionary<BoardPieceId, List<AbilityKey>> _globalEnergyCards;
        private static bool _isActivated;
        private static int _numVigor;
        private static int _numAlags;
        private static int _numEnergy;
        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _energyCards;

        public CardEnergyAdditionOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> energyCards)
        {
            _energyCards = energyCards;
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _energyCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalEnergyCards = _energyCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _numAlags = 0;
            _numEnergy = 0;
            _numVigor = 0;
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

            if (!_globalEnergyCards.TryGetValue(piece.boardPieceId, out var replacementAbilityKeys))
            {
                return;
            }

            int rand;
            AbilityKey replacementAbilityKey;
            int randNum = RandomProvider.GetThreadRandom().Next(101);
            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                if (randNum < 51)
                {
                    // Class cards
                    rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 14, replacementAbilityKeys.Count);
                }
                else if (randNum > 97)
                {
                    if (_numVigor < 2)
                    {
                        // Invisibility and Vigor Potions
                        _numVigor++;
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 23, replacementAbilityKeys.Count - 21);
                    }
                    else if (_numAlags < 2)
                    {
                        // Rejuv and Damage Resist Potions
                        _numAlags++;
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 25, replacementAbilityKeys.Count - 23);
                    }
                    else if (_numEnergy < 2 && !piece.HasEffectState(EffectStateType.ExtraEnergy))
                    {
                        // Energy Potion
                        _numEnergy++;
                        rand = replacementAbilityKeys.Count - 26;
                    }
                    else
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroWarlock || piece.boardPieceId == BoardPieceId.HeroSorcerer)
                        {
                            // Casters can get Magic Potions but not Strength
                            rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 20, replacementAbilityKeys.Count - 14);
                        }
                        else
                        {
                            // Melee can get Strength Potions but not Magic
                            rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 21, replacementAbilityKeys.Count - 15);
                        }
                    }
                }
                else if (randNum > 93)
                {
                    if (_numAlags < 2)
                    {
                        // Rejuv and Damage Resist Potions
                        _numAlags++;
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 25, replacementAbilityKeys.Count - 23);
                    }
                    else if (_numEnergy < 2 && !piece.HasEffectState(EffectStateType.ExtraEnergy))
                    {
                        // Energy Potion
                        _numEnergy++;
                        rand = replacementAbilityKeys.Count - 26;
                    }
                    else
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroWarlock || piece.boardPieceId == BoardPieceId.HeroSorcerer)
                        {
                            // Casters can get Magic Potions but not Strength
                            rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 19, replacementAbilityKeys.Count - 15);
                        }
                        else
                        {
                            // Melee can get Strength Potions but not Magic
                            rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 20, replacementAbilityKeys.Count - 16);
                        }
                    }
                }
                else if (randNum > 90)
                {
                    if (_numEnergy < 2 && !piece.HasEffectState(EffectStateType.ExtraEnergy))
                    {
                        // Energy Potion
                        _numEnergy++;
                        rand = replacementAbilityKeys.Count - 26;
                    }
                    else
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroWarlock || piece.boardPieceId == BoardPieceId.HeroSorcerer)
                        {
                            // Casters can get Magic Potions but not Strength
                            rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 19, replacementAbilityKeys.Count - 15);
                        }
                        else
                        {
                            // Melee can get Strength Potions but not Magic
                            rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 20, replacementAbilityKeys.Count - 16);
                        }
                    }
                }
                else if (randNum > 75)
                {
                    // Very good Potions/Cards
                    if (piece.boardPieceId == BoardPieceId.HeroWarlock || piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        // Casters can get Magic Potions but not Strength
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 19, replacementAbilityKeys.Count - 15);
                    }
                    else
                    {
                        // Melee can get Strength Potions but not Magic
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 20, replacementAbilityKeys.Count - 16);
                    }
                }
                else
                {
                    // Standard cards
                    rand = RandomProvider.GetThreadRandom().Next(0, replacementAbilityKeys.Count - 26);
                }

                replacementAbilityKey = replacementAbilityKeys[rand];
            }
            else
            {
                rand = RandomProvider.GetThreadRandom().Next(0, replacementAbilityKeys.Count);
                replacementAbilityKey = replacementAbilityKeys[rand];
            }

            Traverse.Create(addCardToPieceEvent).Field<AbilityKey>("card").Value = replacementAbilityKey;
        }
    }
}
