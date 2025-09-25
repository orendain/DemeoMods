namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities;
    using Boardgame.TurnOrder;
    using HarmonyLib;
    using HouseRules.Core.Types;
    using Random = UnityEngine.Random;

    public sealed class TurnOrderRandomizedRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero turn order is randomized each new round.";

        private static bool _isActivated;

        public TurnOrderRandomizedRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(Context context)
        {
            _isActivated = true;
        }

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Constructor(typeof(RearrangePlayerTurnOrder), new[] { typeof(TurnQueue) }),
                prefix: new HarmonyMethod(
                    typeof(TurnOrderRandomizedRule),
                    nameof(RearrangePlayerTurnOrder_Constructor_Prefix)));
        }

        private static bool RearrangePlayerTurnOrder_Constructor_Prefix(
            RearrangePlayerTurnOrder __instance,
            TurnQueue turnQueue)
        {
            if (!_isActivated)
            {
                return true;
            }

            var playerPieces = turnQueue.GetPlayerPieces();
            var playerScores = ComputeScores(playerPieces);
            var precomputedPlayerComparison = PrecomputedPieceComparison(playerScores);

            playerPieces.Sort(precomputedPlayerComparison);

            __instance.newTurnOrder = playerPieces.Select(piece => piece.networkID).ToArray();
            Traverse.Create(__instance).Property<bool>("updateNeeded").Value = true;
            return false;
        }

        /// <summary>
        /// Returns a list of (pieceID, score) mappings, where higher scores represent priority in the turn order.
        /// </summary>
        private static Dictionary<int, int> ComputeScores(IEnumerable<Piece> pieces)
        {
            return pieces.ToDictionary(piece => piece.networkID, piece => ComputeScore(piece));
        }

        /// <summary>
        /// Randomizes the score for the specified piece, where a higher score represents priority in the turn order.
        /// </summary>
        private static int ComputeScore(Piece piece)
        {
            var score = Random.Range(2, 101);
            if (piece.IsDowned())
            {
                score = 1;
            }

            return score;
        }

        /// <summary>
        /// Returns a comparison that references the specified (pieceID, score) mappings.
        /// </summary>
        private static Comparison<Piece> PrecomputedPieceComparison(IReadOnlyDictionary<int, int> pieceScores)
        {
            return (piece1, piece2) =>
            {
                if (piece1 == piece2)
                {
                    return 0;
                }

                if (!pieceScores.TryGetValue(piece1.networkID, out var piece1Score))
                {
                    HouseRulesEssentialsBase.LogWarning(
                        $"[TurnOrderOverridden] Could not find a turn order score for piece [{piece1.networkID}]. Resulting order may be unexpected.");
                    return 1;
                }

                if (!pieceScores.TryGetValue(piece2.networkID, out var piece2Score))
                {
                    HouseRulesEssentialsBase.LogWarning(
                        $"[TurnOrderOverridden] Could not find a turn order score for piece [{piece2.networkID}]. Resulting order may be unexpected.");
                    return 1;
                }

                var scoreDifference = piece1Score - piece2Score;

                // Return negative score difference as a high score represents priority in the turn order, whereas a comparison represents that with negative numbers.
                return -scoreDifference;
            };
        }
    }
}
