namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class PieceConfigAdjustedRule : Rule, IConfigWritable<List<PieceConfigAdjustedRule.PieceProperty>>,
        IMultiplayerSafe
    {
        public override string Description => "Piece configuration is adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

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

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceExistingProperties(_originals);
        }

        /// <summary>
        /// Replaces existing PieceConfig properties with those specified.
        /// </summary>
        /// <returns>Previous properties that are now replaced.</returns>
        private static List<PieceProperty> ReplaceExistingProperties(List<PieceProperty> pieceConfigChanges)
        {
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var previousProperties = new List<PieceProperty>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfig = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Piece];

                previousProperties.Add(new PieceProperty
                {
                    Piece = item.Piece,
                    Property = item.Property,
                    Value = Convert.ToSingle(Traverse.Create(pieceConfig).Field(item.Property).GetValue()),
                });

                ModifyPieceConfig(ref pieceConfig, item.Property, item.Value);
                gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Piece] = pieceConfig;
            }

            return previousProperties;
        }

        private static void ModifyPieceConfig(ref PieceConfigData pieceConfig, string property, float value)
        {
            var valueType = Traverse.Create(pieceConfig).Field(property).GetValueType();
            if (valueType == typeof(int))
            {
                AccessTools.FieldRefAccess<PieceConfigData, int>(pieceConfig, property) = (int)value;
                return;
            }

            if (valueType == typeof(float))
            {
                AccessTools.FieldRefAccess<PieceConfigData, float>(pieceConfig, property) = value;
                return;
            }

            throw new ArgumentException($"Can not support property of type: {valueType}");
        }
    }
}
