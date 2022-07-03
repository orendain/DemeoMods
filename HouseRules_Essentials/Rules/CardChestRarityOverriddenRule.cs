namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardChestRarityOverriddenRule : Rule,
        IConfigWritable<Dictionary<AbilityKey, int>>, IMultiplayerSafe
    {
        public override string Description => "Card chest rarity is overridden";

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardChestRarityOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts list of AbilityKeys for cards which should have chest rarity changed.</param>
        public CardChestRarityOverriddenRule(Dictionary<AbilityKey, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, int>();
        }

        public Dictionary<AbilityKey, int> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = UpdateExistingCardConfigs(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            UpdateExistingCardConfigs(_originals);
        }

        private static Dictionary<AbilityKey, int> UpdateExistingCardConfigs(
            Dictionary<AbilityKey, int> cardProperties)
        {
            var gameConfigCardConfigs = Traverse.Create(typeof(GameDataAPI))
                .Field<Dictionary<GameConfigType, List<CardConfigDTO>>>("CardConfigDTOlist").Value;
            var cardConfigs = gameConfigCardConfigs[MotherbrainGlobalVars.CurrentConfig];
            var previousConfigs = new Dictionary<AbilityKey, int>();

            for (var i = 0; i < cardConfigs.Count; i++)
            {
                var cardConfig = cardConfigs[i];
                if (!cardProperties.ContainsKey(cardConfig.Card))
                {
                    continue;
                }

                previousConfigs.Add(cardConfig.Card, cardConfig.ChestRarity);
                cardConfig.ChestRarity = cardProperties[cardConfig.Card];
                cardConfigs[i] = cardConfig;
            }

            return previousConfigs;
        }
    }
}
