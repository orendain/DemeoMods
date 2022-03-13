namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class PiecePieceTypeListOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, PieceType[]>>, IMultiplayerSafe
    {
        public override string Description => "Piece piece types are adjusted";

        private readonly Dictionary<BoardPieceId, PieceType[]> _adjustments;
        private readonly Dictionary<BoardPieceId, PieceType[]> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecePieceTypeListOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and Lists of Behaviours
        /// Replaces original settings with new list.</param>
        public PiecePieceTypeListOverriddenRule(Dictionary<BoardPieceId, PieceType[]> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, PieceType[]>();
        }

        public Dictionary<BoardPieceId, PieceType[]> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{HR.FixBossNames(item.Key)}"));
                _originals[item.Key] = pieceConfig.PieceType;
                var property = Traverse.Create(pieceConfig).Property<PieceType[]>("PieceType");
                property.Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _originals)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{HR.FixBossNames(item.Key)}"));
                var property = Traverse.Create(pieceConfig).Property<PieceType[]>("PieceType");
                property.Value = item.Value;
            }
        }
    }
}
