namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.PlayerData;
    using Data.GameData;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class LevelPropertiesModifiedRule : Rule, IConfigWritable<LevelPropertiesModifiedRule.ConfigData>, IMultiplayerSafe
    {
        public override string Description => "Level properties are modified";

        private const int DefaultDreadLevel = 1;
        private readonly ConfigData _levelProperties;

        public struct ConfigData
        {
            public List<GameConfigType> Limit;
            public Dictionary<string, int> Adjustments;
        }

        public LevelPropertiesModifiedRule(ConfigData levelProperties)
        {
            _levelProperties = levelProperties;
        }

        public ConfigData GetConfigObject() => _levelProperties;

        protected override void OnDeactivate(GameContext gameContext)
        {
            Traverse.Create(gameContext.playerDataController)
                .Field<Dictionary<GameConfigType, PlayerDataController.MergedDreadData[]>>("mergedDreadDataCollection").Value = null;
            Traverse.Create(gameContext.playerDataController).Method("AssembleDreadModesIfNull").GetValue();
        }

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            Traverse.Create(gameContext.playerDataController).Method("AssembleDreadModesIfNull").GetValue();

            var mergedDreadDataCollection = Traverse.Create(gameContext.playerDataController)
                .Field<Dictionary<GameConfigType, PlayerDataController.MergedDreadData[]>>("mergedDreadDataCollection").Value;

            EssentialsMod.Logger.Msg($"LevelProps {MotherbrainGlobalVars.CurrentConfig}");
            EssentialsMod.Logger.Msg($"LevelProps {_levelProperties.Limit}");
            if (_levelProperties.Limit.Contains(MotherbrainGlobalVars.CurrentConfig) || _levelProperties.Limit.Contains(Boardgame.GameConfigType.None))
                {
                EssentialsMod.Logger.Msg($"Modifying props {_levelProperties.Limit}");
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
        }

        private void ModifyDreadMode(ref DreadLevelsDTO dreadLevelDto)
        {
            foreach (var modification in _levelProperties.Adjustments)
            {
                EssentialsMod.Logger.Msg($"{MotherbrainGlobalVars.CurrentConfig}: {modification.Key}: {modification.Value}");
                AccessTools.StructFieldRefAccess<DreadLevelsDTO, int>(ref dreadLevelDto, modification.Key) = modification.Value;
            }
        }
    }
}
