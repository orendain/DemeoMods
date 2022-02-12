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

        public LevelPropertiesModifiedRule(Dictionary<string, int> levelProperties)
        {
            _levelProperties = levelProperties;
        }

        public Dictionary<string, int> GetConfigObject() => _levelProperties;

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

        private void ModifyDreadMode(ref DreadLevelsDTO dreadLevelDto)
        {
            foreach (var modification in _levelProperties)
            {
                AccessTools.StructFieldRefAccess<DreadLevelsDTO, int>(ref dreadLevelDto, modification.Key) = modification.Value;
            }
        }
    }
}
