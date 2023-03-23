namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.PlayerData;
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
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 2 },
                { "FloorOneGoldMaxAmount", 465 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoVillagers", 1 },
                { "FloorTwoLootChests", 4 },
                { "FloorTwoGoldMaxAmount", 560 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 1 },
                { "FloorThreeElvenSummoners", 0 },
        };

        private readonly Dictionary<string, int> _elvenKing = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 2 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 3 },
                { "FloorOneGoldMaxAmount", 465 },
                { "FloorOneElvenSummoners", 1 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoLootChests", 3 },
                { "FloorTwoGoldMaxAmount", 560 },
                { "FloorTwoElvenSummoners", 1 },
                { "FloorTwoBeggars", 1 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 1 },
                { "FloorThreeElvenSummoners", 0 },
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
            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Sewers)
                {
                    foreach (var modification in _ratKing)
                    {
                        AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                           modification.Value;
                    }

                    return;
                }
                else if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Town)
                {
                    foreach (var modification in _elvenKing)
                    {
                        AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                           modification.Value;
                    }

                    return;
                }
            }

            foreach (var modification in _levelProperties)
            {
                int value1 = AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key);
                // EssentialsMod.Logger.Msg($"{modification.Key} = {value1}");
                AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                    modification.Value;
            }
        }
    }
}
