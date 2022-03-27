namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceUseWhenKilledOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>, IMultiplayerSafe
    {
        public override string Description => "Piece UseWhenKilled lists are overridden";

        protected override SyncableTrigger ModifiedData => SyncableTrigger.PieceDataChanged;

        private readonly Dictionary<BoardPieceId, List<AbilityKey>> _adjustments;
        private Dictionary<BoardPieceId, List<AbilityKey>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceUseWhenKilledOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of BoardPieceId and List<AbilityKey> to be applied to UseWhenKilled
        /// Replaces original settings with new list.</param>
        public PieceUseWhenKilledOverriddenRule(Dictionary<BoardPieceId, List<AbilityKey>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, List<AbilityKey>>();
        }

        public Dictionary<BoardPieceId, List<AbilityKey>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
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
        /// <returns>Dictionary of lists of previous PieceConfig properties that are now replaced.</returns>
        private static Dictionary<BoardPieceId, List<AbilityKey>> ReplaceExistingProperties(Dictionary<BoardPieceId, List<AbilityKey>> pieceConfigChanges)
        {
            var gameConfigPieceConfigs = Traverse.Create(typeof(GameDataAPI)).Field<Dictionary<GameConfigType, Dictionary<BoardPieceId, PieceConfigDTO>>>("PieceConfigDTOdict").Value;
            var previousProperties = new Dictionary<BoardPieceId, List<AbilityKey>>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameConfigPieceConfigs[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.UseWhenKilled.ToList();
                pieceConfigDto.UseWhenKilled = item.Value.ToArray();
                gameConfigPieceConfigs[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }

    }
}
