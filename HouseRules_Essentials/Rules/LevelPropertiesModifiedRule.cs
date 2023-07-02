namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.PlayerData;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class LevelPropertiesModifiedRule : Rule, IConfigWritable<Dictionary<string, int>>, IMultiplayerSafe
    {
        public override string Description => "Each floor's POI's and maximum gold are adjusted";

        private const int DefaultDreadLevel = 1;
        private readonly Dictionary<string, int> _levelProperties;
        private readonly Dictionary<string, int> _ratKing = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 20 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 2 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoVillagers", 1 },
                { "FloorTwoLootChests", 4 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 1 },
                { "FloorThreeElvenSummoners", 0 },
        };

        private readonly Dictionary<string, int> _ratKingEasy = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 2 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 5 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoVillagers", 1 },
                { "FloorTwoLootChests", 5 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 1 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 3 },
                { "FloorThreeElvenSummoners", 0 },
        };

        private readonly Dictionary<string, int> _ratKingHard = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 0 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 4 },
                { "FloorOneGoldMaxAmount", 600 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoVillagers", 1 },
                { "FloorTwoLootChests", 5 },
                { "FloorTwoGoldMaxAmount", 750 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 2 },
                { "FloorThreeElvenSummoners", 0 },
                { "PacingSpikeSegmentFloorThreeBudget", 9 },
        };

        private readonly Dictionary<string, int> _ratKingLegendary = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 0 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 6 },
                { "FloorOneGoldMaxAmount", 450 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 0 },
                { "FloorTwoVillagers", 1 },
                { "FloorTwoLootChests", 7 },
                { "FloorTwoGoldMaxAmount", 600 },
                { "FloorTwoElvenSummoners", 1 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 3 },
                { "FloorThreeElvenSummoners", 0 },
                { "PacingSpikeSegmentFloorThreeBudget", 12 },
        };

        private readonly Dictionary<string, int> _elvenKing = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 20 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 3 },
                { "FloorOneElvenSummoners", 1 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoLootChests", 3 },
                { "FloorTwoElvenSummoners", 1 },
                { "FloorTwoBeggars", 1 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 2 },
                { "FloorThreeElvenSummoners", 0 },
        };

        private readonly Dictionary<string, int> _elvenKingEasy = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 2 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 5 },
                { "FloorOneElvenSummoners", 1 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoLootChests", 5 },
                { "FloorTwoElvenSummoners", 1 },
                { "FloorTwoBeggars", 1 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 1 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 3 },
                { "FloorThreeElvenSummoners", 0 },
        };

        private readonly Dictionary<string, int> _elvenKingHard = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 0 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 4 },
                { "FloorOneGoldMaxAmount", 650 },
                { "FloorOneElvenSummoners", 1 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoLootChests", 4 },
                { "FloorTwoGoldMaxAmount", 750 },
                { "FloorTwoElvenSummoners", 1 },
                { "FloorTwoBeggars", 1 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 2 },
                { "FloorThreeElvenSummoners", 0 },
        };

        private readonly Dictionary<string, int> _elvenKingLegendary = new Dictionary<string, int>
        {
                { "BigGoldPileChance", 0 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 6 },
                { "FloorOneGoldMaxAmount", 500 },
                { "FloorOneElvenSummoners", 1 },
                { "FloorOneSellswords", 0 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 0 },
                { "FloorTwoLootChests", 7 },
                { "FloorTwoGoldMaxAmount", 600 },
                { "FloorTwoElvenSummoners", 2 },
                { "FloorTwoBeggars", 1 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 3 },
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
            var ruleSet = HR.SelectedRuleset.Name;
            if (ruleSet.Contains("Demeo Revolutions"))
            {
                if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Sewers)
                {
                    if (ruleSet.Contains("(EASY"))
                    {
                        foreach (var modification in _ratKingEasy)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }
                    else if (ruleSet.Contains("(HARD") || ruleSet.Contains("(PROGRESSIVE"))
                    {
                        foreach (var modification in _ratKingHard)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }
                    else if (ruleSet.Contains("(LEGENDARY"))
                    {
                        foreach (var modification in _ratKingLegendary)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }
                    else
                    {
                        foreach (var modification in _ratKing)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }

                    return;
                }
                else if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Town)
                {
                    if (ruleSet.Contains("(EASY"))
                    {
                        foreach (var modification in _elvenKingEasy)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }
                    else if (ruleSet.Contains("(HARD") || ruleSet.Contains("(PROGRESSIVE"))
                    {
                        foreach (var modification in _elvenKingHard)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }
                    else if (ruleSet.Contains("(LEGENDARY"))
                    {
                        foreach (var modification in _elvenKingLegendary)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }
                    else
                    {
                        foreach (var modification in _elvenKing)
                        {
                            AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                               modification.Value;
                        }
                    }

                    return;
                }
            }

            foreach (var modification in _levelProperties)
            {
                // int value1 = AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key);
                // EssentialsMod.Logger.Msg($"{modification.Key} = {value1}");
                AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                    modification.Value;
            }
        }
    }
}
