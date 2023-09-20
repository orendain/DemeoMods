namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class PieceUseWhenKilledOverriddenRule : Rule,
        IConfigWritable<Dictionary<BoardPieceId, List<AbilityKey>>>, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Piece 'use when killed' abilities are adjusted";

        protected override SyncableTrigger ModifiedSyncables =>
            SyncableTrigger.NewPieceModified | SyncableTrigger.StatusEffectDataModified;

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
        private static Dictionary<BoardPieceId, List<AbilityKey>> ReplaceExistingProperties(
            Dictionary<BoardPieceId, List<AbilityKey>> pieceConfigChanges)
        {
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var previousProperties = new Dictionary<BoardPieceId, List<AbilityKey>>();

            foreach (var item in pieceConfigChanges)
            {
                var pieceConfigDto = gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key];
                previousProperties[item.Key] = pieceConfigDto.UseWhenKilled.ToList();

                // EssentialsMod.Logger.Msg($"Abilities for {item.Key}: {string.Join(", ", previousProperties[item.Key])}"); // Uncomment to see original WhenKilled Abilities
                pieceConfigDto.UseWhenKilled = item.Value.ToArray();
                gameContext.gameDataAPI.PieceConfig[MotherbrainGlobalVars.CurrentConfig][item.Key] = pieceConfigDto;
            }

            return previousProperties;
        }
    }
}
