namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class PieceImmunityListAdjustedRule : Rule, IConfigWritable<Dictionary<string, EffectStateType[]>>, IMultiplayerSafe
    {
        public override string Description => "Piece immunities are adjusted";

        private readonly Dictionary<string, EffectStateType[]> _adjustments;
        private readonly Dictionary<string, EffectStateType[]> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceImmunityListAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and EffectStateType[]
        /// Replaces original settings with new list.</param>
        public PieceImmunityListAdjustedRule(Dictionary<string, EffectStateType[]> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<string, EffectStateType[]>();
        }

        public Dictionary<string, EffectStateType[]> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                _originals[item.Key] = pieceConfig.ImmuneToStatusEffects;
                var property = Traverse.Create(pieceConfig).Property<EffectStateType[]>("ImmuneToStatusEffects");
                property.Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _originals)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item}"));
                var property = Traverse.Create(pieceConfig).Property<EffectStateType[]>("ImmuneToStatusEffects");
                property.Value = item.Value;
            }
        }
    }
}
