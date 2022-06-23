namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardClassRestrictionOverriddenRule : Rule,
        IConfigWritable<Dictionary<AbilityKey, BoardPieceId>>, IMultiplayerSafe
    {
        public override string Description => "Card class restrictions are overridden";

        private readonly Dictionary<AbilityKey, BoardPieceId> _adjustments;
        private Dictionary<AbilityKey, BoardPieceId> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardClassRestrictionOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts list of AbilityKeys for cards which should not be dealt.</param>
        public CardClassRestrictionOverriddenRule(Dictionary<AbilityKey, BoardPieceId> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, BoardPieceId>();
        }

        public Dictionary<AbilityKey, BoardPieceId> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = UpdateExistingCardConfigs(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            UpdateExistingCardConfigs(_originals);
        }

        private static Dictionary<AbilityKey, BoardPieceId> UpdateExistingCardConfigs(
            Dictionary<AbilityKey, BoardPieceId> cardProperties)
        {
            var gameConfigCardConfigs = Traverse.Create(typeof(GameDataAPI))
                .Field<Dictionary<GameConfigType, List<CardConfigDTO>>>("CardConfigDTOlist").Value;
            var cardConfigs = gameConfigCardConfigs[MotherbrainGlobalVars.CurrentConfig];
            var previousConfigs = new Dictionary<AbilityKey, BoardPieceId>();

            for (var i = 0; i < cardConfigs.Count; i++)
            {
                var cardConfig = cardConfigs[i];
                if (!cardProperties.ContainsKey(cardConfig.Card))
                {
                    continue;
                }

                previousConfigs.Add(cardConfig.Card, cardConfig.ClassRestriction);
                cardConfig.ClassRestriction = cardProperties[cardConfig.Card];
                cardConfigs[i] = cardConfig;
            }

            return previousConfigs;
        }
    }
}
