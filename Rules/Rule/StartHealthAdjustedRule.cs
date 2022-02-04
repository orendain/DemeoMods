namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using MelonLoader.TinyJSON;
    using UnityEngine;

    public sealed class StartHealthAdjustedRule : RulesAPI.Rule, RulesAPI.IConfigWritable
    {
        public override string Description => "Start Health is adjusted";

        private readonly Dictionary<string, int> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartHealthAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an entity to the number of action points
        /// added to their base. Negative numbers are allowed.</param>
        public StartHealthAdjustedRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
        }

        public static StartHealthAdjustedRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out Dictionary<string, int> conf);
            return new StartHealthAdjustedRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_adjustments, EncodeOptions.NoTypeHints);
        }

        protected override void OnPostGameCreated()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var property = Traverse.Create(pieceConfig).Property<int>("StartHealth");
                property.Value += item.Value;
            }
        }

        protected override void OnDeactivate()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var property = Traverse.Create(pieceConfig).Property<int>("StartHealth");
                property.Value -= item.Value;
            }
        }
    }
}
