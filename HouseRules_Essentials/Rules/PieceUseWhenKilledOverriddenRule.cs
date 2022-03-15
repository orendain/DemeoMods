namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceUseWhenKilledOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>, IMultiplayerSafe
    {
        public override string Description => "Piece UseWhenKilled lists are overridden";

        protected override SpecialSyncData ModifiedData => SpecialSyncData.StatusEffectImmunity;

        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _adjustments;
        private Dictionary<GameConfigType, Dictionary<BoardPieceId, List<AbilityKey>>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceUseWhenKilledOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of BoardPieceId and List<AbilityKey> to be applied to UseWhenKilled
        /// Replaces original settings with new list.</param>
        public PieceUseWhenKilledOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<GameConfigType, Dictionary<BoardPieceId, List<AbilityKey>>>();
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _adjustments;

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
        private static Dictionary<GameConfigType, Dictionary<BoardPieceId, List<AbilityKey>>> ReplaceExistingProperties(GameConfigType gameConfigType, Dictionary<BoardPieceId, List<AbilityKey>> pieceConfigChanges)
        {
            var gameConfigPieceConfigs = Traverse.Create(typeof(GameDataAPI)).Field<Dictionary<GameConfigType, Dictionary<BoardPieceId, PieceConfigDTO>>>("PieceConfigDTOdict").Value;
            var previousProperties = new Dictionary<GameConfigType, Dictionary<BoardPieceId, List<AbilityKey>>>
            {
                [MotherbrainGlobalVars.CurrentConfig] = new Dictionary<BoardPieceId, List<AbilityKey>>(),
            };

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameConfigPieceConfigs[gameConfigType][item.Key];
                var property = Traverse.Create(pieceConfigDto).Property<List<AbilityKey>>("UseWhenKilled");

                previousProperties[gameConfigType][item.Key] = property.Value;
                pieceConfigDto.UseWhenKilled = item.Value.ToArray();
                gameConfigPieceConfigs[gameConfigType][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }

    }
}
