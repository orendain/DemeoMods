namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

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

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceExistingProperties(_originals);
        }

        private static Dictionary<BoardPieceId, List<PieceType>> ReplaceExistingProperties(
            Dictionary<BoardPieceId, List<PieceType>> pieceConfigChanges)
        {
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var previousProperties = new Dictionary<BoardPieceId, List<PieceType>>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.PieceType.ToList();
                pieceConfigDto.PieceType = item.Value.ToArray();
                gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }
    }
}
