namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.Data;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardChestAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card loot from chests is adjusted";

        private static Dictionary<BoardPieceId, List<AbilityKey>> _globalchestCards;
        private static bool _isActivated;
        private static int _numPlayers;
        private static bool _isChest;
        private static int _numVigor;
        private static int _numAlags;
        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _chestCards;

        public CardChestAdditionOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> chestCards)
        {
            _chestCards = chestCards;
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _chestCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalchestCards = _chestCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _numAlags = 0;
            _isActivated = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Interactable), "OnInteraction", new[] { typeof(int), typeof(IntPoint2D), typeof(GameContext), typeof(int) }),
                prefix: new HarmonyMethod(
                    typeof(CardChestAdditionOverriddenRule),
                    nameof(Interactable_OnInteraction_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                prefix: new HarmonyMethod(
                    typeof(CardChestAdditionOverriddenRule),
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

        private static void Interactable_OnInteraction_Prefix(
            GameContext gameContext,
            IntPoint2D targetTile)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!gameContext.pieceAndTurnController.GetInteractableAtPosition(targetTile))
            {
                return;
            }

            Interactable whatIsit = gameContext.pieceAndTurnController.GetInteractableAtPosition(targetTile);
            if (whatIsit.type == Interactable.Type.Chest)
            {
                _numPlayers = gameContext.pieceAndTurnController.GetNumberOfPlayerPieces();
                _isChest = true;
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

            if (!_isChest)
            {
                return;
            }

            if (_numPlayers > 1)
            {
                _numPlayers--;
            }
            else
            {
                _isChest = false;
            }

            if (request.type != SerializableEvent.Type.AddCardToPiece)
            {
                return;
            }

            var addCardToPieceEvent = (SerializableEventAddCardToPiece)request;
            var gameContext = Traverse.Create(__instance).Property<GameContext>("gameContext").Value;
            var pieceId = Traverse.Create(addCardToPieceEvent).Field<int>("pieceId").Value;

            if (!gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            if (!_globalchestCards.TryGetValue(piece.boardPieceId, out var replacementAbilityKeys))
            {
                return;
            }

            int rand;
            AbilityKey replacementAbilityKey;
            int randNum = RandomProvider.GetThreadRandom().Next(101);
            if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                if (randNum < 67)
                {
                    // Class cards
                    rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 15, replacementAbilityKeys.Count);
                }
                else if (randNum > 97)
                {
                    if (_numVigor < 2)
                    {
                        // Invisibility and Vigor Potions
                        _numVigor++;
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 21, replacementAbilityKeys.Count - 19);
                    }
                    else if (_numAlags < 2)
                    {
                        // Rejuv and Damage Resist Potions
                        _numAlags++;
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 23, replacementAbilityKeys.Count - 21);
                    }
                    else
                    {
                        // Very good Potions/Cards
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 19, replacementAbilityKeys.Count - 15);
                    }
                }
                else if (randNum > 93)
                {
                    if (_numAlags < 2)
                    {
                        // Rejuv and Damage Resist Potions
                        _numAlags++;
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 23, replacementAbilityKeys.Count - 21);
                    }
                    else
                    {
                        // Very good Potions/Cards
                        rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 19, replacementAbilityKeys.Count - 15);
                    }
                }
                else if (randNum > 75)
                {
                    // Very good Potions/Cards
                    rand = RandomProvider.GetThreadRandom().Next(replacementAbilityKeys.Count - 19, replacementAbilityKeys.Count - 15);
                }
                else
                {
                    // Standard cards 8% chance
                    rand = RandomProvider.GetThreadRandom().Next(0, replacementAbilityKeys.Count - 23);
                }

                replacementAbilityKey = replacementAbilityKeys[rand];
            }
            else
            {
                rand = RandomProvider.GetThreadRandom().Next(0, replacementAbilityKeys.Count);
                replacementAbilityKey = replacementAbilityKeys[rand];
            }

            Traverse.Create(addCardToPieceEvent).Field<AbilityKey>("card").Value = replacementAbilityKey;
            return;
        }
    }
}
