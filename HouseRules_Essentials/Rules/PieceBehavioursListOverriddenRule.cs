﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using Behaviour = DataKeys.Behaviour;

    public sealed class PieceBehavioursListOverriddenRule : Rule,
        IConfigWritable<Dictionary<BoardPieceId, List<Behaviour>>>, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Piece behaviours are adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private readonly Dictionary<BoardPieceId, List<Behaviour>> _adjustments;
        private Dictionary<BoardPieceId, List<Behaviour>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceBehavioursListOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and Lists of Behaviours
        /// Replaces original settings with new list.</param>
        public PieceBehavioursListOverriddenRule(Dictionary<BoardPieceId, List<Behaviour>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, List<Behaviour>>();
        }

        public Dictionary<BoardPieceId, List<Behaviour>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceExistingProperties(_originals);
        }

        private static Dictionary<BoardPieceId, List<Behaviour>> ReplaceExistingProperties(
            Dictionary<BoardPieceId, List<Behaviour>> pieceConfigChanges)
        {
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var previousProperties = new Dictionary<BoardPieceId, List<Behaviour>>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.Behaviours.ToList();
                pieceConfigDto.Behaviours = item.Value.ToArray();
                gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }
    }
}
