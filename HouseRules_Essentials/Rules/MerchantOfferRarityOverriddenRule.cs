namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class MerchantOfferRarityOverriddenRule : Rule,
        IConfigWritable<Dictionary<AbilityKey, int>>
    {
        public override string Description => "Merchant card rarity is modified <Skirmish only>";

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantOfferRarityOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts list of AbilityKeys for cards which should have merchant rarity changed.</param>
        public MerchantOfferRarityOverriddenRule(Dictionary<AbilityKey, int> adjustments)
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
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;

            var cardConfigs = gameContext.gameDataAPI.CardConfig[MotherbrainGlobalVars.CurrentConfig];
            var previousConfigs = new Dictionary<AbilityKey, int>();

            foreach (var cardProperty in cardProperties)
            {
                // MelonLoader.MelonLogger.Msg($"Changed from -> {cardConfigs[cardProperty.Key].Card}: {cardConfigs[cardProperty.Key].MerchantOfferRarity}");
                previousConfigs.Add(cardConfigs[cardProperty.Key].Card, cardConfigs[cardProperty.Key].MerchantOfferRarity);
                cardConfigs[cardProperty.Key].MerchantOfferRarity = cardProperty.Value;
            }

            return previousConfigs;
        }
    }
}
