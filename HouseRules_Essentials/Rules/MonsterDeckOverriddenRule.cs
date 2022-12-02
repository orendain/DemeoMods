namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.AIDirector;
    using Boardgame.Board;
    using Boardgame.BoardEntities;
    using Boardgame.Data;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using Utils;

    public sealed class MonsterDeckOverriddenRule : Rule, IConfigWritable<MonsterDeckOverriddenRule.DeckConfig>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "MonsterDeck creation is overriden";

        private static MonsterDeckOverriddenRule.DeckConfig _globalAdjustments;
        private static bool _isActivated;

        private readonly MonsterDeckOverriddenRule.DeckConfig _adjustments;

        public struct DeckConfig
        {
            public Dictionary<BoardPieceId, int> EntranceDeckFloor1;
            public Dictionary<BoardPieceId, int> ExitDeckFloor1;
            public Dictionary<BoardPieceId, int> EntranceDeckFloor2;
            public Dictionary<BoardPieceId, int> ExitDeckFloor2;
            public Dictionary<BoardPieceId, int> BossDeck;
            public BoardPieceId KeyHolderFloor1;
            public BoardPieceId KeyHolderFloor2;
            public BoardPieceId Boss;
        }

        public MonsterDeckOverriddenRule(MonsterDeckOverriddenRule.DeckConfig adjustments)
        {
            _adjustments = adjustments;
        }

        public MonsterDeckOverriddenRule.DeckConfig GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(AIDirectorDeckConstructor), "ConstructMonsterDeck"),
                prefix: new HarmonyMethod(
                    typeof(MonsterDeckOverriddenRule),
                    nameof(AIDirectorDeckConstructor_ConstructMonsterDeck_Prefix)));
            harmony.Patch(
                original: AccessTools.Method(typeof(AIDirectorController2), "SpawnBossAndMinions"),
                prefix: new HarmonyMethod(
                    typeof(MonsterDeckOverriddenRule),
                    nameof(AIDirectorController2_SpawnBossAndMinions_Prefix)));
        }

        private static List<MonsterDeck.MonsterDeckEntry> CreateSubDeck(Dictionary<BoardPieceId, int> subdeck)
        {
            var mySubDeck = new List<MonsterDeck.MonsterDeckEntry>();
            foreach (var deckItemConfig in subdeck)
            {
                var deckItem = new MonsterDeck.MonsterDeckEntry
                {
                    BoardPieceId = deckItemConfig.Key,
                    enemyWeight = EnemyWeight.Light,
                    isRedrawEnabled = false,
                };
                if (deckItemConfig.Value == 0)
                {
                    deckItem.isRedrawEnabled = true;
                }

                mySubDeck.Add(deckItem);
                for (int i = 0; i < deckItemConfig.Value - 1; i++)
                {
                    mySubDeck.Add(deckItem);
                }
            }

            return mySubDeck;
        }

        private static bool AIDirectorDeckConstructor_ConstructMonsterDeck_Prefix(ref MonsterDeck __result, int floorIndex, IRnd rng)
        {
            if (!_isActivated)
            {
                return true;
            }

            List<MonsterDeck.MonsterDeckEntry> standardDeck;
            List<MonsterDeck.MonsterDeckEntry> spikeDeck;
            MonsterDeck.MonsterDeckEntry keyHolder;
            if (floorIndex == 1)
            {
                standardDeck = CreateSubDeck(_globalAdjustments.EntranceDeckFloor1);
                spikeDeck = CreateSubDeck(_globalAdjustments.ExitDeckFloor1);
                keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor1, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            }
            else
            {
                standardDeck = CreateSubDeck(_globalAdjustments.EntranceDeckFloor2);
                spikeDeck = CreateSubDeck(_globalAdjustments.ExitDeckFloor2);
                keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor2, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            }

            List<MonsterDeck.MonsterDeckEntry> bossDeck = CreateSubDeck(_globalAdjustments.BossDeck);

            MonsterDeck monsterDeck = new MonsterDeck
            {
                monsterDeckStandard = standardDeck,
                monsterDeckSpike = spikeDeck,
                monsterDeckBoss = bossDeck,
                Keyholder = keyHolder,
            };
            monsterDeck.ShuffleSubDecks(rng);
            __result = monsterDeck;
            return false; // We returned an user-adjusted config.
        }

        private static bool AIDirectorController2_SpawnBossAndMinions_Prefix(ref AIDirectorContext context, ref TransientBoardState boardState)
        {
            if (!_isActivated)
            {
                return true;
            }

            SpawnZoneTag tag = SpawnZoneTag.Zone2;
            SpawnZone spawnZone = null;
            SpawnZone spawnZone2 = null;
            List<SpawnZone> list;
            list = context.spawnZoneModel.GetZonesWithTag(tag);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                SpawnZone spawnZone3 = list[i];
                if (spawnZone3.GetAllFreeTiles(ref boardState).Count == 0)
                {
                    EssentialsMod.Logger.Msg(string.Format("Spawn zone doesn't contain any free tiles, removing it! {0}", spawnZone3.TileRect));
                    list.RemoveAt(i);
                }
            }

            int num = 0;
            for (int j = 0; j < list.Count; j++)
            {
                SpawnZone spawnZone4 = list[j];
                if (spawnZone4.NumStepsToExit >= AIDirectorConfig.KeyHolderMinDistanceToExit && spawnZone4.NumStepsToExit <= AIDirectorConfig.KeyHolderMaxDistanceToExit && spawnZone4.numStepsToEntrance >= AIDirectorConfig.KeyHolderMinDistanceToEntrance)
                {
                    List<IntPoint2D> allFreeTiles = spawnZone4.GetAllFreeTiles(ref boardState);
                    if (allFreeTiles.Count > 2)
                    {
                        spawnZone = spawnZone4;
                        break;
                    }

                    EssentialsMod.Logger.Msg(string.Concat(new string[]
                    {
                        "Spawnzone had only ",
                        allFreeTiles.Count.ToString(),
                        " free tiles! name:",
                        spawnZone4.Zone.name,
                        " position:",
                        spawnZone4.Zone.centerWorldPosition.ToString(),
                        " map name:",
                        MotherbrainGlobalVars.CurrentConfig.ToString(),
                    }));
                }
                else
                {
                    num++;
                    if (spawnZone2 == null)
                    {
                        spawnZone2 = spawnZone4;
                    }
                }
            }

            if (spawnZone == null && spawnZone2 != null)
            {
                spawnZone = spawnZone2;
            }

            if (spawnZone == null)
            {
                EssentialsMod.Logger.Msg("MD: Could not find a spawn zone to spawn the boss");
            }

            PieceSpawnSettings spawnSettings = new PieceSpawnSettings(_globalAdjustments.Boss, Team.Two);
            context.spawner.SpawnPiece(context, spawnSettings, ref boardState);
            return false; // We returned an user-adjusted config.
        }
    }
}
