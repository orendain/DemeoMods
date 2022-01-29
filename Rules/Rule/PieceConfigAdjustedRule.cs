namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using UnityEngine;

    public sealed class PieceConfigAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Start Health is adjusted";

        private readonly List<List<string>> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceConfigAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Lists of PieceNames, PropertyNames and Values
        /// added to their base. Negative numbers are allowed.</param>
        public PieceConfigAdjustedRule(List<List<string>> adjustments)
        {
            _adjustments = adjustments;
        }

        protected override void OnPostGameCreated()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item[0]}"));
                var actionPoint = Traverse.Create(pieceConfig).Property<int>(item[1]);
                actionPoint.Value += int.Parse(item[2]);
            }
        }

        protected override void OnDeactivate()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item[0]}"));
                var actionPoint = Traverse.Create(pieceConfig).Property<int>(item[1]);
                actionPoint.Value -= int.Parse(item[2]);
            }
        }
    }
}
