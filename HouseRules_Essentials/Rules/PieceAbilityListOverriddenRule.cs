namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class PieceAbilityListOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>, IMultiplayerSafe
    {
        public override string Description => "Piece abilities are adjusted";

        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _adjustments;
        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceAbilityListOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and Lists of abilityKeys
        /// Replaces original settings with new list.</param>
        public PieceAbilityListOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, List<AbilityKey>>();
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{HR.FixBossNames(item.Key)}"));
                _originals[item.Key] = pieceConfig.Abilities;
                var property = Traverse.Create(pieceConfig).Property<List<AbilityKey>>("Abilities");
                property.Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _originals)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{HR.FixBossNames(item.Key)}"));
                var property = Traverse.Create(pieceConfig).Property<List<AbilityKey>>("Abilities");
                property.Value = item.Value;
            }
        }
    }
}
