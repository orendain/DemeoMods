namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class StartCardsModifiedRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero start cards are modified";

        private static Dictionary<BoardPieceId, List<CardConfig>> _globalHeroStartCards;
        private static bool _isActivated;

        private readonly Dictionary<BoardPieceId, List<CardConfig>> _heroStartCards;

        public struct CardConfig
        {
            public AbilityKey Card;
            public int IsReplenishable;
        }

        public StartCardsModifiedRule(Dictionary<BoardPieceId, List<CardConfig>> heroStartCards)
        {
            ValidateCards(heroStartCards);
            _heroStartCards = heroStartCards;
        }

        public Dictionary<BoardPieceId, List<CardConfig>> GetConfigObject() => _heroStartCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalHeroStartCards = _heroStartCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
                    nameof(Piece_CreatePiece_Postfix)));
            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static bool Inventory_RestoreReplenishables_Prefix(ref bool __result, Piece piece)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = false;
            for (int i = 0; i < piece.inventory.Items.Count; i++)
            {
                Inventory.Item value = piece.inventory.Items[i];
                var targetRefresh = (value.flags >> 5) & 7;
                var countdown = (value.flags >> 2) & 7;
                if (piece.inventory.Items[i].IsReplenishing && (!piece.HasEffectState(EffectStateType.Stealthed) || piece.inventory.Items[i].abilityKey != AbilityKey.Sneak))
                {
                    // If countdown was zero when we got called, then we need to set it.
                    if (countdown == 0)
                    {
                        countdown = targetRefresh;
                    }

                    // If we reached our desired turn count we can unset isReplenishing and return true
                    if (countdown == 1)
                    {
                        value.flags &= -3; // unsets isReplenishing (bit1 ) allowing card to be used again.
                        __result = true;
                    }

                    countdown -= 1;
                    value.flags &= 227; // Zero only the countdown bits using a bitmask
                    value.flags |= countdown << 2; // OR with countdown to set them again.
                    piece.inventory.Items[i] = value;
                    Traverse.Create(piece.inventory).Property<bool>("needSync").Value = true;
                }
            }

            return false;
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!_globalHeroStartCards.ContainsKey(__result.boardPieceId))
            {
                return;
            }

            SetInventory(__result);
        }

        private static void SetInventory(Piece piece)
        {
            piece.inventory.Items.Clear();
            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 0;

            foreach (var card in _globalHeroStartCards[piece.boardPieceId])
            {
                // flag bits
                // 0 : isReplenishable
                // 1 : isReplenishing
                // 2-4 : ReplenishCounter - 3-bit range used by RestoreReplenishables for counting rounds.
                // 5-7 : ReplenishFrequency - 3-bit number, user-configured target.
                int flags = 0;
                if (card.IsReplenishable > 0)
                {
                    flags = 1;
                    int refreshFrequency = (card.IsReplenishable > 7) ? 7 : card.IsReplenishable; // Limit to max of 7 turns.
                    flags |= refreshFrequency << 5; // logical or with refreshFrequency shifted 5 bits to the left to become ReplenishFrequency bits 5-7
                }

                piece.inventory.Items.Add(new Inventory.Item
                {
                    abilityKey = card.Card,
                    flags = flags,
                    originalOwner = -1,
                });
                Traverse.Create(piece.inventory).Property<bool>("needSync").Value = true;
            }
        }

        private static void ValidateCards(Dictionary<BoardPieceId, List<CardConfig>> heroStartCards)
        {
            foreach (var startCards in heroStartCards.Values)
            {
                if (startCards.Count(c => c.IsReplenishable > 0) > 2)
                {
                    throw new ArgumentException("Only 2 replenishable cards allowed.");
                }
            }
        }
    }
}
