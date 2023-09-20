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
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class CardEnergyAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card loot from energy (mana) is adjusted";

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

            int rand;
            AbilityKey replacementAbilityKey;
            int randNum = RandomProvider.GetThreadRandom().Next(101);
            if ((piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Revolutions") || HR.SelectedRuleset.Name.Equals("TEST GAME")) && !HR.SelectedRuleset.Name.Contains("PROGRESSIVE"))
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
