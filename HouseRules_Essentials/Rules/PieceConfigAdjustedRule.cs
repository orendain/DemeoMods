namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceConfigAdjustedRule : Rule, IConfigWritable<List<PieceConfigAdjustedRule.PieceProperty>>, IMultiplayerSafe
    {
        public override string Description => "Piece configuration is adjusted";

        private readonly List<PieceProperty> _adjustments;
        private Dictionary<GameConfigType, List<PieceProperty>> _originals;

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
            _originals = new Dictionary<GameConfigType, List<PieceProperty>>();
        }

        public List<PieceProperty> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(MotherbrainGlobalVars.CurrentConfig, _adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            foreach (var gameConfigType in _originals)
            {
                ReplaceExistingProperties(gameConfigType.Key, _originals[gameConfigType.Key]);
            }
        }

        /// <summary>
        /// Replaces existing PieceConfig properties with those specified.
        /// </summary>
        /// <returns>Dictionary of GameConfigYypes and lists of previous PieceConfig properties that are now replaced.</returns>
        private static Dictionary<GameConfigType, List<PieceProperty>> ReplaceExistingProperties(GameConfigType gameConfigType, List<PieceProperty> pieceConfigChanges)
        {
            var gameConfigPieceConfigs = Traverse.Create(typeof(GameDataAPI)).Field<Dictionary<GameConfigType, Dictionary<BoardPieceId, PieceConfigDTO>>>("PieceConfigDTOdict").Value;
            var previousProperties = new Dictionary<GameConfigType, List<PieceProperty>>
            {
                [MotherbrainGlobalVars.CurrentConfig] = new List<PieceProperty>(),
            };

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameConfigPieceConfigs[gameConfigType][item.Piece];
                var propertyTraverse = Traverse.Create(pieceConfigDto).Field(item.Property);

                previousProperties[gameConfigType].Add(new PieceProperty
                {
                    Piece = item.Piece,
                    Property = item.Property,
                    Value = Convert.ToSingle(propertyTraverse.GetValue()),
                });

                ModifyPieceConfig(ref pieceConfigDto, item.Property, item.Value);
                gameConfigPieceConfigs[gameConfigType][item.Piece] = pieceConfigDto;
            }

            return previousProperties;
        }

        private static void ModifyPieceConfig(ref PieceConfigDTO pieceConfigDto, string property, float value)
        {
            var valueType = Traverse.Create(pieceConfigDto).Field(property).GetValueType();
            if (valueType == typeof(int))
            {
                AccessTools.StructFieldRefAccess<PieceConfigDTO, int>(ref pieceConfigDto, property) = (int)value;
                return;
            }

            if (valueType == typeof(float))
            {
                AccessTools.StructFieldRefAccess<PieceConfigDTO, float>(ref pieceConfigDto, property) = value;
                return;
            }

            throw new ArgumentException($"Can not support property of type: {valueType}");
        }
    }
}
