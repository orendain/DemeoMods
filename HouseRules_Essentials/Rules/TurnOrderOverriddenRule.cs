namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.TurnOrder;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class TurnOrderOverriddenRule : Rule, IConfigWritable<TurnOrderOverriddenRule.Scores>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Turn order is overridden.";

        private static Scores _globalConfig;
        private static bool _isActivated;

        private readonly Scores _config;

        public struct Scores
        {
            public int Assassin;
            public int Bard;
            public int Guardian;
            public int Hunter;
            public int Sorcerer;
            public int Warlock;
            public int Downed;
            public int Javelin;
        }

        public TurnOrderOverriddenRule(Scores scores)
        {
            _config = scores;
        }

        public Scores GetConfigObject() => _config;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalConfig = _config;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Constructor(typeof(RearrangePlayerTurnOrder), new[] { typeof(TurnQueue) }),
                prefix: new HarmonyMethod(
                    typeof(TurnOrderOverriddenRule),
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
            var playerScores = ComputeScores(playerPieces, _globalConfig);
            var precomputedPlayerComparison = PrecomputedPieceComparison(playerScores);

            playerPieces.Sort(precomputedPlayerComparison);

            __instance.newTurnOrder = playerPieces.Select(piece => piece.networkID).ToArray();
            Traverse.Create(__instance).Property<bool>("updateNeeded").Value = true;
            return false;
        }

        /// <summary>
        /// Returns a list of (pieceID, score) mappings, where higher scores represent priority in the turn order.
        /// </summary>
        private static Dictionary<int, int> ComputeScores(IEnumerable<Piece> pieces, Scores scores)
        {
            return pieces.ToDictionary(piece => piece.networkID, piece => ComputeScore(piece, scores));
        }

        /// <summary>
        /// Computes the score for the specified piece, where a higher score represents priority in the turn order.
        /// </summary>
        private static int ComputeScore(Piece piece, Scores scores)
        {
            var score = 0;

            switch (piece.boardPieceId)
            {
                case BoardPieceId.HeroBard:
                    score += scores.Bard;
                    break;
                case BoardPieceId.HeroGuardian:
                    score += scores.Guardian;
                    break;
                case BoardPieceId.HeroHunter:
                    score += scores.Hunter;
                    break;
                case BoardPieceId.HeroSorcerer:
                    score += scores.Sorcerer;
                    break;
                case BoardPieceId.HeroRogue:
                    score += scores.Assassin;
                    break;
                case BoardPieceId.HeroWarlock:
                    score += scores.Warlock;
                    break;
            }

            if (piece.IsDowned())
            {
                score += scores.Downed;
            }

            if (piece.inventory.HasAbility(AbilityKey.SigataurianJavelin) && MotherbrainGlobalVars.CurrentConfig == GameConfigType.Forest)
            {
                score += scores.Javelin;
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
                    EssentialsMod.Logger.Warning(
                        $"[TurnOrderOverridden] Could not find a turn order score for piece [{piece1.networkID}]. Resulting order may be unexpected.");
                    return 1;
                }

                if (!pieceScores.TryGetValue(piece2.networkID, out var piece2Score))
                {
                    EssentialsMod.Logger.Warning(
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
