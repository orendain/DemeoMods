namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class SpawnCategoryOverriddenRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<int>>>, IMultiplayerSafe
    {
        public override string Description => "Enemy spawn randomness is adjusted";

        private readonly Dictionary<BoardPieceId, List<int>> _adjustments;
        private Dictionary<BoardPieceId, List<int>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnCategoryOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts a Dictionary of BoardPieceIDs and Int settings
        /// for MaxPerDeck.</param>
        public SpawnCategoryOverriddenRule(Dictionary<BoardPieceId, List<int>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<BoardPieceId, List<int>>();
        }

        public Dictionary<BoardPieceId, List<int>> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(Context context)
        {
            _originals = UpdateSpawnCategories(_adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            UpdateSpawnCategories(_originals);
        }

        private static Dictionary<BoardPieceId, List<int>> UpdateSpawnCategories(Dictionary<BoardPieceId, List<int>> spawnModifications)
        {
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var spawnCategories = gameContext.gameDataAPI.SpawnCategory[MotherbrainGlobalVars.CurrentConfig];
            var previousConfigs = new Dictionary<BoardPieceId, List<int>>();
            var bpis = new List<BoardPieceId>();
            foreach (var nap in spawnModifications)
            {
                bpis.Add(nap.Key);
            }

            for (var i = 0; i < spawnCategories.Count; i++)
            {
                if (bpis.Contains(spawnCategories[i].BoardPieceId))
                {
                    previousConfigs[spawnCategories[i].BoardPieceId] = new List<int>
                    {
                        spawnCategories[i].MaxPerDeck,
                        spawnCategories[i].PreFill,
                        spawnCategories[i].FirstAllowedLevelIndex,
                    };

                    spawnCategories[i] = new SpawnCategoryData
                    {
                        BoardPieceId = spawnCategories[i].BoardPieceId,
                        EnemyWeight = spawnCategories[i].EnemyWeight,
                        IsSpawningEnabled = true,
                        IsAllowedKeyholder = spawnCategories[i].IsAllowedKeyholder,
                        IsBossSynergyUnit = spawnCategories[i].IsBossSynergyUnit,
                        OverrideDefaultMaxPerDeckBehaviour = true,
                        MaxPerDeck = spawnModifications[spawnCategories[i].BoardPieceId][0],
                        PreFill = spawnModifications[spawnCategories[i].BoardPieceId][1],
                        FirstAllowedLevelIndex = spawnModifications[spawnCategories[i].BoardPieceId][2],
                        IsRedrawEnabled = spawnCategories[i].IsRedrawEnabled,
                        IsPriorityUnit = true,
                    };
                }
                else
                {
                    spawnCategories[i] = new SpawnCategoryData
                    {
                        BoardPieceId = spawnCategories[i].BoardPieceId,
                        EnemyWeight = spawnCategories[i].EnemyWeight,
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
                }
            }

            return previousConfigs;
        }
    }
}
