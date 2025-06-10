namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using DataKeys;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class PiecePieceTypeListOverriddenRule : Rule,
        IConfigWritable<Dictionary<BoardPieceId, List<PieceType>>>, IMultiplayerSafe
    {
        public override string Description => "Piece types are adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private readonly Dictionary<BoardPieceId, List<PieceType>> _adjustments;
        private Dictionary<BoardPieceId, List<PieceType>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecePieceTypeListOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and Lists of Behaviours
        /// Replaces original settings with new list.</param>
        public PiecePieceTypeListOverriddenRule(Dictionary<BoardPieceId, List<PieceType>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, List<PieceType>>();
        }

        public Dictionary<BoardPieceId, List<PieceType>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceExistingProperties(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceExistingProperties(context, _originals);
        }

        private static Dictionary<BoardPieceId, List<PieceType>> ReplaceExistingProperties(
            Context context,
            Dictionary<BoardPieceId, List<PieceType>> pieceConfigChanges)
        {
            var previousProperties = new Dictionary<BoardPieceId, List<PieceType>>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = context.GameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.PieceType.ToList();
                pieceConfigDto.PieceType = item.Value.ToArray();
                context.GameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }
    }
}
