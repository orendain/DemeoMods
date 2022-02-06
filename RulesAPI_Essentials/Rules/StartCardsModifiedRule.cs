namespace RulesAPI.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;

    public sealed class StartCardsModifiedRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>>, IPatchable
    {
        public override string Description => "Hero start cards are modified";

        private static Dictionary<BoardPieceId, List<CardConfig>> _heroStartCards;
        private static bool _isActivated;

        public struct CardConfig
        {
            public AbilityKey Card;
            public bool IsReplenishable;
        }

        public StartCardsModifiedRule(Dictionary<BoardPieceId, List<CardConfig>> heroStartCards)
        {
            ValidateCards(heroStartCards);
            _heroStartCards = heroStartCards;
        }

        public Dictionary<BoardPieceId, List<CardConfig>> GetConfigObject() => _heroStartCards;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
                    nameof(Piece_CreatePiece_Postfix)));
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (__result.boardPieceId != BoardPieceId.HeroBard &&
                __result.boardPieceId != BoardPieceId.HeroGuardian &&
                __result.boardPieceId != BoardPieceId.HeroHunter &&
                __result.boardPieceId != BoardPieceId.HeroRouge &&
                __result.boardPieceId != BoardPieceId.HeroSorcerer)
            {
                return;
            }

            SetInventory(__result);
        }

        private static void SetInventory(Piece piece)
        {
            piece.inventory.Items.Clear();
            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 0;

            foreach (var card in _heroStartCards[piece.boardPieceId])
            {
                piece.TryAddAbilityToInventory(card.Card, isReplenishable: card.IsReplenishable);
            }
        }

        private static void ValidateCards(Dictionary<BoardPieceId, List<CardConfig>> heroStartCards)
        {
            foreach (var startCards in heroStartCards.Values)
            {
                if (startCards.Count(c => c.IsReplenishable) > 2)
                {
                    throw new ArgumentException("Only 2 replenishable cards allowed.");
                }
            }
        }
    }
}
