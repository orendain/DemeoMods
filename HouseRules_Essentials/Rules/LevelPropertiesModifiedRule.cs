namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.PlayerData;
    using Data.GameData;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class LevelPropertiesModifiedRule : Rule, IConfigWritable<Dictionary<string, int>>, IMultiplayerSafe
    {
        public override string Description => "Level properties are modified";

        private const int DefaultDreadLevel = 1;
        private readonly Dictionary<string, int> _levelProperties;
        private readonly Dictionary<string, int> _ratKing = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 1 },
                { "FloorOneLootChests", 3 },
                { "FloorOneGoldMaxAmount", 700 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoLootChests", 3 },
                { "FloorTwoGoldMaxAmount", 900 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 2 },
        };

        public LevelPropertiesModifiedRule(Dictionary<string, int> levelProperties)
        {
            _levelProperties = levelProperties;
        }

        public Dictionary<string, int> GetConfigObject() => _levelProperties;

        protected override void OnDeactivate(GameContext gameContext)
        {
            Traverse.Create(gameContext.playerDataController)
                .Field<Dictionary<GameConfigType, PlayerDataController.MergedDreadData[]>>("mergedDreadDataCollection")
                .Value = null;
            Traverse.Create(gameContext.playerDataController).Method("AssembleDreadModesIfNull").GetValue();
        }

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            Traverse.Create(gameContext.playerDataController).Method("AssembleDreadModesIfNull").GetValue();

            var mergedDreadDataCollection = Traverse.Create(gameContext.playerDataController)
                .Field<Dictionary<GameConfigType, PlayerDataController.MergedDreadData[]>>("mergedDreadDataCollection")
                .Value;

            foreach (var mergedDreadData in mergedDreadDataCollection.Values)
            {
                for (var i = 0; i < mergedDreadData.Length; i++)
                {
                    if (mergedDreadData[i].dto.DreadLevel == DefaultDreadLevel)
                    {
                        ModifyDreadMode(ref mergedDreadData[i].dto);
                    }
                }
            }
        }

        private void ModifyDreadMode(ref DreadLevelsData dreadLevel)
        {
            if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Sewers)
            {
                foreach (var modification in _ratKing)
                {
                    AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                       modification.Value;
                }
            }
            else
            {
                foreach (var modification in _levelProperties)
                {
                    AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                       modification.Value;
                }
            }
        }
    }
}
