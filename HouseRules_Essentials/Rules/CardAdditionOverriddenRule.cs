﻿namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.Data;
    using Boardgame.BoardEntities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card additions are overridden";

        private static readonly Random Rnd = new Random();
        private static Dictionary<BoardPieceId, List<AbilityKey>> _globalHeroCards;
        private static bool _isActivated;
        private static bool _isPotionStand = false;
        private static bool _isChest = false;

        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _heroCards;

        public CardAdditionOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> heroCards)
        {
            _heroCards = heroCards;
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _heroCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalHeroCards = _heroCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Interactable), "OnInteraction"),
                prefix: new HarmonyMethod(
                    typeof(CardAdditionOverriddenRule),
                    nameof(Interactable_OnInteraction_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                prefix: new HarmonyMethod(
                    typeof(CardAdditionOverriddenRule),
                    nameof(SerializableEventQueue_RespondToRequest_Prefix)));
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
            if (whatIsit.type == Interactable.Type.PotionStand)
            {
                _isPotionStand = true;
            }
            else if (whatIsit.type == Interactable.Type.Chest)
            {
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

            if (request.type != SerializableEvent.Type.AddCardToPiece)
            {
                return;
            }

            if (_isPotionStand)
            {
                // TODO: Add method to allow custom card loot for Potion Stands here
                return;
            }

            if (!_isChest)
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

            if (!_globalHeroCards.TryGetValue(piece.boardPieceId, out var replacementAbilityKeys))
            {
                return;
            }

            var replacementAbilityKey = replacementAbilityKeys.ElementAt(Rnd.Next(replacementAbilityKeys.Count));
            Traverse.Create(addCardToPieceEvent).Field<AbilityKey>("card").Value = replacementAbilityKey;
        }
    }
}
