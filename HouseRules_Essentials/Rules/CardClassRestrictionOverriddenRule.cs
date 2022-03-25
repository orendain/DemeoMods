namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardClassRestrictionOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, BoardPieceId>>, IMultiplayerSafe
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

        private static Dictionary<AbilityKey, BoardPieceId> UpdateExistingCardConfigs(Dictionary<AbilityKey, BoardPieceId> cardProperties)
        {
            var gameConfigCardConfigs = Traverse.Create(typeof(GameDataAPI)).Field<Dictionary<GameConfigType, List<CardConfigDTO>>>("CardConfigDTOlist").Value;
            var cardConfigs = gameConfigCardConfigs[MotherbrainGlobalVars.CurrentConfig];
            var previousConfigs = new Dictionary<AbilityKey, BoardPieceId>();
            var aks = new List<AbilityKey>();
            foreach (var ak in cardProperties)
            {
                aks.Add(ak.Key);
            }

            for (int i = 0; i < cardConfigs.Count; i++)
            {
                if (aks.Contains(cardConfigs[i].Card))
                {
                    previousConfigs[cardConfigs[i].Card] = cardConfigs[i].ClassRestriction;
                    cardConfigs[i] = new CardConfigDTO
                    {
                        Card = cardConfigs[i].Card,
                        ClassRestriction = cardProperties[cardConfigs[i].Card],
                        Tags = cardConfigs[i].Tags,
                        CostInShop = cardConfigs[i].CostInShop,
                        SellValue = cardConfigs[i].SellValue,
                        TestValue = cardConfigs[i].TestValue,
                        ShopRarity = cardConfigs[i].ShopRarity,
                        ChestRarity = cardConfigs[i].ChestRarity,
                        ClassRarity = cardConfigs[i].ClassRarity,
                        CardEnergy = cardConfigs[i].CardEnergy,
                    };
                }
            }

            return previousConfigs;
        }
    }
}
