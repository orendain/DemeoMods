namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class SpawnCategoryOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<int>>>, IMultiplayerSafe
    {
        public override string Description => "Spawn Category definitions are Overridden";

        private readonly Dictionary<BoardPieceId, List<int>> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnCategoryOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts a Dictionary of BoardPieceIDs and Int settings
        /// for MaxPerDe</param>
        public SpawnCategoryOverriddenRule(Dictionary<BoardPieceId, List<int>> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<BoardPieceId, List<int>> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var spawnCategories = Traverse.Create(typeof(GameDataAPI)).Field<Dictionary<GameConfigType, List<SpawnCategoryDTO>>>("SpawnCategoryDTOlist").Value;
            var bpis = new List<BoardPieceId>();
            foreach (var nap in _adjustments)
            {
                bpis.Add(nap.Key);
            }

            foreach (var gameConfigType in spawnCategories)
            {
                for (int i = 0; i < spawnCategories[gameConfigType.Key].Count; i++)
                {
                    if (bpis.Contains(spawnCategories[gameConfigType.Key][i].BoardPieceId))
                    {
                        spawnCategories[gameConfigType.Key][i] = new SpawnCategoryDTO
                        {
                            BoardPieceId = spawnCategories[gameConfigType.Key][i].BoardPieceId,
                            EnemyWeight = spawnCategories[gameConfigType.Key][i].EnemyWeight,
                            IsSpawningEnabled = true,
                            IsAllowedKeyholder = spawnCategories[gameConfigType.Key][i].IsAllowedKeyholder,
                            IsBossSynergyUnit = spawnCategories[gameConfigType.Key][i].IsBossSynergyUnit,
                            OverrideDefaultMaxPerDeckBehaviour = true,
                            MaxPerDeck = _adjustments[spawnCategories[gameConfigType.Key][i].BoardPieceId][0],
                            PreFill = _adjustments[spawnCategories[gameConfigType.Key][i].BoardPieceId][1],
                            FirstAllowedLevelIndex = _adjustments[spawnCategories[gameConfigType.Key][i].BoardPieceId][2],
                            IsRedrawEnabled = spawnCategories[gameConfigType.Key][i].IsRedrawEnabled,
                            IsPriorityUnit = true,
                        };
                        EssentialsMod.Logger.Warning($"We have enabled {gameConfigType.Key} {spawnCategories[gameConfigType.Key][i].BoardPieceId}");
                    }
                    else
                    {
                        spawnCategories[gameConfigType.Key][i] = new SpawnCategoryDTO
                        {
                            BoardPieceId = spawnCategories[gameConfigType.Key][i].BoardPieceId,
                            EnemyWeight = spawnCategories[gameConfigType.Key][i].EnemyWeight,
                            IsSpawningEnabled = false,
                            IsAllowedKeyholder = false,
                            IsBossSynergyUnit = false,
                            OverrideDefaultMaxPerDeckBehaviour = true,
                            MaxPerDeck = 0,
                            PreFill = 0,
                            FirstAllowedLevelIndex = 4,
                            IsRedrawEnabled = true,
                            IsPriorityUnit = false,
                        };
                        EssentialsMod.Logger.Warning($"We disabled spawning for {gameConfigType.Key} {spawnCategories[gameConfigType.Key][i].BoardPieceId}");
                    }
                }
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var spawnCategories = Traverse.Create(typeof(GameDataAPI)).Field<Dictionary<GameConfigType, List<SpawnCategoryDTO>>>("SpawnCategoryDTOlist").Value;
            foreach (var item in spawnCategories)
            {
                EssentialsMod.Logger.Warning($"We removed a thing {item.Key}");
            }
        }
    }
}



