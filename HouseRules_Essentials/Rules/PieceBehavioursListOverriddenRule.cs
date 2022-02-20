namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;
    using Behaviour = DataKeys.Behaviour;

    public sealed class PieceBehavioursListOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, Behaviour[]>>, IMultiplayerSafe
    {
        public override string Description => "Piece behaviours are adjusted";

        private readonly Dictionary<BoardPieceId, Behaviour[]> _adjustments;
        private readonly Dictionary<BoardPieceId, Behaviour[]> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceBehavioursListOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and Lists of Behaviours
        /// Replaces original settings with new list.</param>
        public PieceBehavioursListOverriddenRule(Dictionary<BoardPieceId, Behaviour[]> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, Behaviour[]>();
        }

        public Dictionary<BoardPieceId, Behaviour[]> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                _originals[item.Key] = pieceConfig.Behaviours;
                var property = Traverse.Create(pieceConfig).Property<Behaviour[]>("Behaviours");
                property.Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _originals)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var property = Traverse.Create(pieceConfig).Property<Behaviour[]>("Behaviours");
                property.Value = item.Value;
            }
        }
    }
}
