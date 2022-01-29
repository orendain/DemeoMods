namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using UnityEngine;

    public sealed class MoveRangeAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Start Health is adjusted";

        private readonly Dictionary<string, int> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveRangehAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an entity to the number of action points
        /// added to their base. Negative numbers are allowed.</param>
        public MoveRangeAdjustedRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
        }

        protected override void OnPostGameCreated()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var actionPoint = Traverse.Create(pieceConfig).Property<int>("MoveRange");
                actionPoint.Value += item.Value;
            }
        }

        protected override void OnDeactivate()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var actionPoint = Traverse.Create(pieceConfig).Property<int>("MoveRange");
                actionPoint.Value -= item.Value;
            }
        }
    }
}
