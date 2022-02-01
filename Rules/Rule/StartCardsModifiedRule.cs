namespace Rules.Rule
{
    using System;
    using System.Collections.Generic;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;

    public sealed class StartCardsModifiedRule : RulesAPI.Rule
    {
        public override string Description => "Zap starting inventory is adjusted";

        private static bool _isActivated;

        private static Dictionary<BoardPieceId, List<(AbilityKey, bool)>> _heroCards;

        public StartCardsModifiedRule(Dictionary<string, List<(string, bool)>> herosCards)
        {
            _heroCards = ParseHeroesCards(herosCards);
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void OnPatch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(typeof(StartCardsModifiedRule), nameof(Piece_CreatePiece_Postfix)));
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!_heroCards.TryGetValue(__result.boardPieceId, out var cards))
            {
                return;
            }

            ClearInventory(__result);
            SetInventory(__result, cards);
        }

        private static void ClearInventory(Piece piece)
        {
            piece.inventory.Items.Clear();
            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 0;
        }

        private static void SetInventory(Piece piece, List<(AbilityKey, bool)> cards)
        {
            foreach (var (ability, isReplenishable) in cards)
            {
                piece.TryAddAbilityToInventory(ability, isReplenishable: isReplenishable);
            }
        }

        private static Dictionary<BoardPieceId, List<(AbilityKey, bool)>> ParseHeroesCards(Dictionary<string, List<(string, bool)>> heroesCards)
        {
            var parseHeroesCards = new Dictionary<BoardPieceId, List<(AbilityKey, bool)>>();
            foreach (var heroCards in heroesCards)
            {
                if (!Enum.TryParse(heroCards.Key, true, out BoardPieceId parsedPieceId))
                {
                    throw new ArgumentException($"Failed to recognize hero: {heroCards.Key}");
                }

                parseHeroesCards.Add(parsedPieceId, ParseCards(heroCards.Value));
            }

            return parseHeroesCards;
        }

        private static List<(AbilityKey, bool)> ParseCards(List<(string, bool)> cards)
        {
            var replenishableCount = 0;
            var parsedCards = new List<(AbilityKey, bool)>();
            foreach (var (ability, isReplenishable) in cards)
            {
                if (isReplenishable && ++replenishableCount > 2)
                {
                    throw new ArgumentException("No more than 2 replenishable cards are allowed.");
                }

                if (!Enum.TryParse(ability, true, out AbilityKey parsedAbility))
                {
                    throw new ArgumentException($"Failed to recognize ability: {ability}");
                }

                parsedCards.Add((parsedAbility, isReplenishable));
            }

            return parsedCards;
        }
    }
}
