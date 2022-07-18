namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceImmunityListAdjustedRule : Rule,
        IConfigWritable<Dictionary<BoardPieceId, List<EffectStateType>>>, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Piece immunities are adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.StatusEffectImmunityModified;

        private readonly Dictionary<BoardPieceId, List<EffectStateType>> _adjustments;
        private Dictionary<BoardPieceId, List<EffectStateType>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceImmunityListAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and List.<EffectStateType>
        /// Replaces original settings with new list.</param>
        public PieceImmunityListAdjustedRule(Dictionary<BoardPieceId, List<EffectStateType>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, List<EffectStateType>>();
        }

        public Dictionary<BoardPieceId, List<EffectStateType>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceExistingProperties(_originals);
        }

        private static Dictionary<BoardPieceId, List<EffectStateType>> ReplaceExistingProperties(
            Dictionary<BoardPieceId, List<EffectStateType>> pieceConfigChanges)
        {
            var gameConfigPieceConfigs = Traverse.Create(typeof(GameDataAPI))
                .Field<Dictionary<GameConfigType, Dictionary<BoardPieceId, PieceConfigDTO>>>("PieceConfigDTOdict")
                .Value;
            var previousProperties = new Dictionary<BoardPieceId, List<EffectStateType>>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameConfigPieceConfigs[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.ImmuneToStatusEffects.ToList();
                pieceConfigDto.ImmuneToStatusEffects = item.Value.ToArray();
                gameConfigPieceConfigs[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }
    }
}
