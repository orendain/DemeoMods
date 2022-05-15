namespace TurnOrderCustomizer
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities;
    using DataKeys;
    using MelonLoader;

    internal class PieceScorer
    {
        private const string PrependedConfigDescription = "Directions: Add numbers to each of the attributes below. Players with higher totals go first in the turn order.\n";

        private readonly Dictionary<BoardPieceId, int> _pieceScores;
        private readonly int _downedScore;
        private readonly int _javelinScore;

        internal bool IsEnabled { get; }

        internal static PieceScorer FromMelonConfig()
        {
            var configCategory = MelonPreferences.CreateCategory("TurnOrderCustomizer");
            var enabledEntry = configCategory.CreateEntry("enabled", description: $"{PrependedConfigDescription}Whether or not (true/false) to override the game's default turn order.", default_value: false);
            var assassinScoreEntry = configCategory.CreateEntry("assassin", 0);
            var bardScoreEntry = configCategory.CreateEntry("bard", 0);
            var guardianScoreEntry = configCategory.CreateEntry("guardian", 0);
            var hunterScoreEntry = configCategory.CreateEntry("hunter", 0);
            var sorcererScoreEntry = configCategory.CreateEntry("sorcerer", 0);
            var downedScoreEntry = configCategory.CreateEntry("downed", description: "Downed players.", default_value: 0);
            var javelinScoreEntry = configCategory.CreateEntry("javelin", description: "Players with the javelin.", default_value: 0);

            var isEnabled = enabledEntry.Value;
            var boardPieceScores = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroRogue, assassinScoreEntry.Value },
                { BoardPieceId.HeroBard, bardScoreEntry.Value },
                { BoardPieceId.HeroGuardian, guardianScoreEntry.Value },
                { BoardPieceId.HeroHunter, hunterScoreEntry.Value },
                { BoardPieceId.HeroSorcerer, sorcererScoreEntry.Value },
            };
            var downedScore = downedScoreEntry.Value;
            var javelinScore = javelinScoreEntry.Value;

            return new PieceScorer(isEnabled, boardPieceScores, downedScore, javelinScore);
        }

        private PieceScorer(bool isEnabled, Dictionary<BoardPieceId, int> pieceScores, int downedScore, int javelinScore)
        {
            IsEnabled = isEnabled;
            _pieceScores = pieceScores;
            _downedScore = downedScore;
            _javelinScore = javelinScore;
        }

        /// <summary>
        /// Returns a list of (pieceID, score) mappings, where higher scores represent priority in the turn order.
        /// </summary>
        internal Dictionary<int, int> ComputeScores(IEnumerable<Piece> pieces)
        {
            return pieces.ToDictionary(piece => piece.networkID, ComputeScore);
        }

        /// <summary>
        /// Computes the score for the specified piece, where a higher score represents priority in the turn order.
        /// </summary>
        private int ComputeScore(Piece piece)
        {
            var score = 0;

            if (_pieceScores.TryGetValue(piece.boardPieceId, out var pieceScore))
            {
                score += pieceScore;
            }

            if (piece.IsDowned())
            {
                score += _downedScore;
            }

            if (piece.inventory.HasAbility(AbilityKey.SigataurianJavelin))
            {
                score += _javelinScore;
            }

            return score;
        }
    }
}
