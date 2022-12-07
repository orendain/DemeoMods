namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class ApplyEffectOnHitAdjustedRule : Rule,
        IConfigWritable<Dictionary<BoardPieceId, EffectStateType>>, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Effects applied on hit are adjusted";

        private readonly Dictionary<BoardPieceId, EffectStateType> _adjustments;
        private Dictionary<BoardPieceId, EffectStateType> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyEffectOnHitAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and effect.<EffectStateType>
        /// Replaces original settings with new effect.</param>
        public ApplyEffectOnHitAdjustedRule(Dictionary<BoardPieceId, EffectStateType> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, EffectStateType>();
        }

        public Dictionary<BoardPieceId, EffectStateType> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceExistingProperties(_originals);
        }

        private static Dictionary<BoardPieceId, EffectStateType> ReplaceExistingProperties(
            Dictionary<BoardPieceId, EffectStateType> pieceConfigChanges)
        {
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var previousProperties = new Dictionary<BoardPieceId, EffectStateType>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.ApplyEffectOnHit;
                pieceConfigDto.ApplyEffectOnHit = item.Value;
                gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }
    }
}
