namespace RulesAPI.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Data.GameData;
    using HarmonyLib;

    public sealed class LevelPropertiesModifiedRule : Rule, IConfigWritable<Dictionary<string, int>>
    {
        public override string Description => "Level properties are modified";

        private const int IndexOfDreadLevelOne = 0;
        private readonly Dictionary<string, int> _levelProperties;
        private readonly List<DreadLevelsDTO> _defaultLevelProperties;

        public LevelPropertiesModifiedRule(Dictionary<string, int> levelProperties)
        {
            _levelProperties = levelProperties;
            _defaultLevelProperties = new List<DreadLevelsDTO>();
        }

        public Dictionary<string, int> GetConfigObject() => _levelProperties;

        protected override void OnDeactivate()
        {
            var allLevelProperties = Traverse.Create<GameDataAPI>()
                .Field<Dictionary<GameConfigType, List<DreadLevelsDTO>>>("DreadLevelsDTOlist").Value.Values;

            for (var i = 0; i < allLevelProperties.Count; i++)
            {
                var levelProperties = allLevelProperties.ElementAt(i);
                levelProperties.Insert(IndexOfDreadLevelOne, _defaultLevelProperties[i]);
            }
        }

        protected override void OnPreGameCreated()
        {
            var allLevelProperties = Traverse.Create<GameDataAPI>()
                .Field<Dictionary<GameConfigType, List<DreadLevelsDTO>>>("DreadLevelsDTOlist").Value.Values;

            for (var i = 0; i < allLevelProperties.Count; i++)
            {
                var levelProperties = allLevelProperties.ElementAt(i);
                var defaultLevelProperties = levelProperties[IndexOfDreadLevelOne];
                _defaultLevelProperties.Insert(i, defaultLevelProperties);
                ModifyDefaultDreadMode(ref defaultLevelProperties);
                levelProperties.Insert(IndexOfDreadLevelOne, defaultLevelProperties);
            }
        }

        private void ModifyDefaultDreadMode(ref DreadLevelsDTO dreadLevelDto)
        {
            foreach (var modification in _levelProperties)
            {
                AccessTools.StructFieldRefAccess<DreadLevelsDTO, int>(ref dreadLevelDto, modification.Key) = modification.Value;
            }
        }
    }
}
