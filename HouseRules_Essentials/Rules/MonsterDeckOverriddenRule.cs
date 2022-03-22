namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.AIDirector;
    using Boardgame.Board;
    using Boardgame.BoardEntities;
    using Boardgame.Data;
    using Boardgame.LayerCake;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using Utils;

    public sealed class MonsterDeckOverriddenRule : Rule, IConfigWritable<MonsterDeckOverriddenRule.DeckConfig>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "SubDeckOverriddenRule does stuff.";

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
                original: AccessTools.Method(typeof(AIDirectorDeckConstructor), "AddPreFillUnitsToDeck"),
                prefix: new HarmonyMethod(
                    typeof(MonsterDeckOverriddenRule),
                    nameof(AIDirectorDeckConstructor_AddPreFillUnitsToDeck_Prefix)));
            harmony.Patch(
                original: AccessTools.Method(typeof(ZoneSpawner), "GetLampTypes"),
                prefix: new HarmonyMethod(
                    typeof(MonsterDeckOverriddenRule),
                    nameof(ZoneSpawner_GetLampTypes_Prefix)));
            harmony.Patch(
                original: AccessTools.Method(typeof(AIDirectorController2), "SpawnBossAndMinions"),
                prefix: new HarmonyMethod(
                    typeof(MonsterDeckOverriddenRule),
                    nameof(AIDirectorController2_SpawnBossAndMinions_Prefix)));
        }

        private static List<MonsterDeck.MonsterDeckEntry> CreateSubDeck(Dictionary<BoardPieceId, int> subdeck)
        {
            var mySubDeck = new List<MonsterDeck.MonsterDeckEntry>() { };
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
                for (int i = 0; i < Math.Max(0, deckItemConfig.Value - 1); i++)
                {
                    mySubDeck.Add(deckItem);
                }
            }

            return mySubDeck;
        }

        private static bool AIDirectorDeckConstructor_ConstructMonsterDeck_Prefix(ref MonsterDeck __result, ISpawnCategoryProvider spawnCategoryProvider, int floorIndex, IRnd rng, LevelSequence.GameType gameType)
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

        private static bool AIDirectorDeckConstructor_AddPreFillUnitsToDeck_Prefix(List<MonsterDeck.MonsterDeckEntry> monsterDeckList, ISpawnCategoryProvider spawnCategoryProvider)
        {
            if (!_isActivated)
            {
                return true;
            }

            return false; // We did all that we needed to do, and that was nothing.
        }

        private static bool ZoneSpawner_GetLampTypes_Prefix(ref BoardPieceId[] __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = new BoardPieceId[]
                    {
                        BoardPieceId.OilLamp,
                        BoardPieceId.IceLamp,
                        BoardPieceId.GasLamp,
                        BoardPieceId.VortexLamp,
                    };

            return false; // We returned an user-adjusted config.
        }

        private static bool AIDirectorController2_SpawnBossAndMinions_Prefix(ref AIDirectorController2 __instance, ref AIDirectorContext context, IRnd rng, ref TransientBoardState boardState)
        {
            if (!_isActivated)
            {
                return true;
            }

            EssentialsMod.Logger.Msg($"MD: AI Director context: {context.gameContext}");
            EssentialsMod.Logger.Msg($"MD: RNG Value: {rng.Value}");
            EssentialsMod.Logger.Msg($"MD: Boardstate Width: {boardState.Width}");
            EssentialsMod.Logger.Msg($"MD: Boardstate Height: {boardState.Height}");
            SpawnZoneTag tag = SpawnZoneTag.Zone2;
            object pickSpawnZoneForKeyholderResult = Traverse.Create(__instance).Method("PickSpawnZoneForKeyholder", context, rng, boardState, tag).GetValue();
            EssentialsMod.Logger.Msg($"MD: Methods: {Traverse.Create(__instance).Methods()}");
            var spawnZone = Traverse.Create(pickSpawnZoneForKeyholderResult).Field<SpawnZone>("spawnZone").Value;
            var keyHolderPosition = Traverse.Create(pickSpawnZoneForKeyholderResult).Field<IntPoint2D>("keyHolderPosition").Value;
            var jimpos = new IntPoint2D
            {
                x = 20,
                y = 20,
            };
            // var backupKeyHolderPosition = Traverse.Create(pickSpawnZoneForKeyholderResult).Field<IntPoint2D>("backupKeyHolderPosition").Value;
            EssentialsMod.Logger.Msg($"MD: SpawnZone: {spawnZone}");
            EssentialsMod.Logger.Msg($"MD: keyHolderPosition: {keyHolderPosition}");


            if (spawnZone == null)
            {
                EssentialsMod.Logger.Msg("MD: Could not find a spawn zone to spawn the boss");
            }

            PieceSpawnSettings spawnSettings = new PieceSpawnSettings(_globalAdjustments.Boss, jimpos, 0f, 0, Team.None).SetRandomRotation(rng).SetHasBloodhound(PieceSpawnSettings.BloodHoundStatus.Enabled).AddEffectState(EffectStateType.AIDirectorAmbientEnemy).AddEffectState(EffectStateType.UnitLeader).AddEffectState(EffectStateType.KeyEndChest);
            context.spawner.SpawnPiece(context, spawnSettings, ref boardState);
            return false; // We returned an user-adjusted config.
        }
    }
}
