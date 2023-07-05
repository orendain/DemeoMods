﻿namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Boardgame;
    using Boardgame.Data;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardChestAdditionOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card additions from chests are adjusted";

        private static Dictionary<BoardPieceId, List<AbilityKey>> _globalChestCards;
        private static bool _isActivated;
        private static bool _isChest;
        private static int _numPlayers;

        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _chestCards;

        public CardChestAdditionOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> chestCards)
        {
            _chestCards = chestCards;
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _chestCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalChestCards = _chestCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
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

            if (_isChest)
            {
                if (_numPlayers > 1)
                {
                    _numPlayers--;
                }
                else
                {
                    _isChest = false;
                }
            }
            else
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

            if (!gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            if (!_globalChestCards.TryGetValue(piece.boardPieceId, out var replacementAbilityKeys))
            {
                return;
            }

            var rand = RandomProvider.GetThreadRandom().Next(0, replacementAbilityKeys.Count);
            var replacementAbilityKey = replacementAbilityKeys[rand];
            Traverse.Create(addCardToPieceEvent).Field<AbilityKey>("card").Value = replacementAbilityKey;
        }
    }
}
