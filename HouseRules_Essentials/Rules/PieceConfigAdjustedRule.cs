namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class PieceConfigAdjustedRule : Rule, IConfigWritable<List<PieceConfigAdjustedRule.PieceProperty>>, IMultiplayerSafe
    {
        public override string Description => "Piece configuration is adjusted";

        private readonly List<PieceProperty> _adjustments;
        private List<PieceProperty> _originals;

        public struct PieceProperty
        {
            public BoardPieceId Piece;
            public string Property;
            public float Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceConfigAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">List of <see cref="PieceProperty"/> that represent PieceConfig properties to replace.</param>
        public PieceConfigAdjustedRule(List<PieceProperty> adjustments)
        {
            _adjustments = adjustments;
            _originals = new List<PieceProperty>();
        }

        public List<PieceProperty> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceExistingProperties(_originals);
        }

        /// <summary>
        /// Replaces existing PieceConfig properties with those specified.
        /// </summary>
        /// <returns>The list of previous PieceConfig properties that are now replaced.</returns>
        private static List<PieceProperty> ReplaceExistingProperties(List<PieceProperty> pieceProperties)
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            var previousProperties = new List<PieceProperty>();
            foreach (var item in pieceProperties)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{HR.FixBossNames(item.Piece)}"));
                var propertyTraverse = Traverse.Create(pieceConfig).Property(item.Property);
                var castedNewValue = CastPropertyValue(item.Value, propertyTraverse.GetValueType());

                previousProperties.Add(new PieceProperty
                {
                    Piece = item.Piece,
                    Property = item.Property,
                    Value = Convert.ToSingle(propertyTraverse.GetValue()),
                });

                propertyTraverse.SetValue(castedNewValue);
            }

            return previousProperties;
        }

        private static object CastPropertyValue(float value, Type propertyValueType)
        {
            if (propertyValueType == typeof(int))
            {
                return (int)value;
            }

            if (propertyValueType == typeof(float))
            {
                return value;
            }

            throw new ArgumentException($"Can not support a piece property of type: {propertyValueType}");
        }
    }
}
