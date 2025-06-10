namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.PlayerData;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class LevelPropertiesModifiedRule : Rule, IConfigWritable<Dictionary<string, int>>, IMultiplayerSafe
    {
        public override string Description => "Each floor's POI's and maximum gold are adjusted";

        private const int DefaultDreadLevel = 1;
        private readonly Dictionary<string, int> _levelProperties;

        public LevelPropertiesModifiedRule(Dictionary<string, int> levelProperties)
        {
            _levelProperties = levelProperties;
        }

        public Dictionary<string, int> GetConfigObject() => _levelProperties;

        protected override void OnDeactivate(Context context)
        {
            Traverse.Create(context.GameContext.playerDataController)
                .Field<Dictionary<GameConfigType, PlayerDataController.MergedDreadData[]>>("mergedDreadDataCollection")
                .Value = null;
            Traverse.Create(context.GameContext.playerDataController).Method("AssembleDreadModesIfNull").GetValue();
        }

        protected override void OnPreGameCreated(Context context)
        {
            Traverse.Create(context.GameContext.playerDataController).Method("AssembleDreadModesIfNull").GetValue();

            var mergedDreadDataCollection = Traverse.Create(context.GameContext.playerDataController)
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
            foreach (var modification in _levelProperties)
            {
                AccessTools.FieldRefAccess<DreadLevelsData, int>(dreadLevel, modification.Key) =
                   modification.Value;
            }
        }
    }
}
