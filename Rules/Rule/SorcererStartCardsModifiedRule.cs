namespace Rules.Rule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class SorcererStartCardsModifiedRule : RulesAPI.Rule, RulesAPI.IConfigWritable, RulesAPI.IPatchable
    {
        public override string Description => "Sorcerer start cards are modified";

        private static List<Card> _cards;
        private static bool _isActivated;

        public struct Card
        {
            public AbilityKey Name;
            public bool IsReplenishable;
        }

        public SorcererStartCardsModifiedRule(List<Card> cards)
        {
            if (cards.Count(c => c.IsReplenishable) > 2)
            {
                throw new ArgumentException("Only 2 replenishable cards allowed.");
            }

            _cards = cards;
        }

        public static SorcererStartCardsModifiedRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out List<Card> conf);
            return new SorcererStartCardsModifiedRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_cards, EncodeOptions.NoTypeHints);
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(typeof(SorcererStartCardsModifiedRule), nameof(Piece_CreatePiece_Postfix)));
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (__result.boardPieceId != BoardPieceId.HeroSorcerer)
            {
                return;
            }

            SetInventory(__result);
        }

        private static void SetInventory(Piece piece)
        {
            piece.inventory.Items.Clear();
            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 0;

            foreach (var card in _cards)
            {
                piece.TryAddAbilityToInventory(card.Name, isReplenishable: card.IsReplenishable);
            }
        }
    }
}
