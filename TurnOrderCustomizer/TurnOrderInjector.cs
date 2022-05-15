namespace TurnOrderCustomizer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities;
    using Boardgame.TurnOrder;
    using DataKeys;
    using HarmonyLib;

    internal static class TurnOrderInjector
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Constructor(typeof(RearrangePlayerTurnOrder), new[] { typeof(TurnQueue) }),
                prefix: new HarmonyMethod(typeof(TurnOrderInjector), nameof(RearrangePlayerTurnOrder_Constructor_Prefix)));
        }

        private static bool RearrangePlayerTurnOrder_Constructor_Prefix(
            RearrangePlayerTurnOrder __instance,
            TurnQueue turnQueue)
        {
            if (!TurnOrderCustomizerMod.PieceScorer.IsEnabled)
            {
                return true;
            }

            var playerPieces = turnQueue.GetPlayerPieces();
            var playerScores = TurnOrderCustomizerMod.PieceScorer.ComputeScores(playerPieces);
            var precomputedPlayerComparison = PrecomputedPieceComparison(playerScores);

            playerPieces.Sort(precomputedPlayerComparison);

            __instance.newTurnOrder = playerPieces.Select(piece => piece.networkID).ToArray();
            Traverse.Create(__instance).Property<bool>("updateNeeded").Value = true;
            return false;
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
                    TurnOrderCustomizerMod.Logger.Warning(
                        $"Could not find a turn order score for piece [{piece1.networkID}]. Resulting order may be unexpected.");
                    return 1;
                }

                if (!pieceScores.TryGetValue(piece2.networkID, out var piece2Score))
                {
                    TurnOrderCustomizerMod.Logger.Warning(
                        $"Could not find a turn order score for piece [{piece2.networkID}]. Resulting order may be unexpected.");
                    return 1;
                }

                var scoreDifference = piece1Score - piece2Score;

                // Return negative score difference as a high score represents priority in the turn order, whereas a comparison represents that with negative numbers.
                return -scoreDifference;
            };
        }

        /// <summary>
        /// Returns a comparison representing the default Demeo piece comparison.
        /// </summary>
        /// <remarks>Unused, but included for reference.</remarks>
        private static Comparison<Piece> DefaultPieceComparison()
        {
            return (piece1, piece2) =>
            {
                if (piece1 == piece2)
                {
                    return 0;
                }

                var score = (piece1.PlayerSpawnedIndex < piece2.PlayerSpawnedIndex) ? -1 : 1;

                if (piece1.HasEffectState(EffectStateType.Downed))
                {
                    score += 2;
                }

                if (piece2.HasEffectState(EffectStateType.Downed))
                {
                    score -= 2;
                }

                return score;
            };
        }
    }
}
