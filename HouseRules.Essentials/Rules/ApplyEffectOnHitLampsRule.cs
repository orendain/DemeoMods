namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class ApplyEffectOnHitLampsRule : Rule, /// bobthebunny
        IConfigWritable<Dictionary<BoardPieceId, EffectStateType>>, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Lamps apply effects when getting damaged";

        private readonly Dictionary<BoardPieceId, EffectStateType> _adjustments;
        private Dictionary<BoardPieceId, EffectStateType> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyEffectOnHitLampsRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of piece name and effect.<EffectStateType>
        /// Replaces original settings with new effect.</param>
        public ApplyEffectOnHitLampsRule(Dictionary<BoardPieceId, EffectStateType> adjustments)
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

            /// foreach (var item in pieceConfigChanges)
            /// {
            ///     var pieceConfigDto = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key];
            ///     if (item.BoardPieceId == BoardPieceId.GasLamp)
            ///     {
            ///         previousProperties[item.Key] = pieceConfigDto.ApplyEffectOnHit;
            ///         pieceConfigDto.ApplyEffectOnHit = EffectStateType.Antidote;
            ///         gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            ///     }
            /// }
            foreach (var item in pieceConfigChanges) /// goes through the current game objects.
            {
                var pieceConfigDto = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key]; /// access the current game context's value. its PieceConfig.
                previousProperties[item.Key] = pieceConfigDto.ApplyEffectOnHit; /// 
                pieceConfigDto.ApplyEffectOnHit = item.Value; /// 
                gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto; /// 
            }

            return previousProperties;
        }
    }
}
