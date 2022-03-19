namespace HouseRules.Essentials.Rules
{
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

    public sealed class MonsterDeckOverriddenRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "SubDeckOverriddenRule does stuff.";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public MonsterDeckOverriddenRule(List<BoardPieceId> adjustments)
        {
            _adjustments = adjustments;
        }

        public List<BoardPieceId> GetConfigObject() => _adjustments;

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

        private static bool AIDirectorDeckConstructor_ConstructMonsterDeck_Prefix(ref MonsterDeck __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            var alice = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.IceElemental, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            var bob = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.ChestGoblin, enemyWeight = EnemyWeight.Medium, isRedrawEnabled = false };
            var chris = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.FireElemental, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            var dave = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.RootGolem, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            var edgar = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.Spider, enemyWeight = EnemyWeight.Light, isRedrawEnabled = true };
            var fran = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.RootMage, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            var myDeck = new List<MonsterDeck.MonsterDeckEntry>() { alice, bob, bob, bob, bob, bob, chris, dave, edgar, fran };
            MonsterDeck monsterDeck = new MonsterDeck
            {
                monsterDeckStandard = myDeck,
                monsterDeckSpike = myDeck,
                monsterDeckBoss = myDeck,
                Keyholder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = BoardPieceId.Cavetroll, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false },
            };
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
            object pickSpawnZoneForKeyholderResult = Traverse.Create(__instance).Method("PickSpawnZoneForKeyholder", context, rng, boardState, tag);
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

            PieceSpawnSettings spawnSettings = new PieceSpawnSettings(BoardPieceId.MotherCy, jimpos, 0f, 0, Team.None).SetRandomRotation(rng).SetHasBloodhound(PieceSpawnSettings.BloodHoundStatus.Enabled).AddEffectState(EffectStateType.AIDirectorAmbientEnemy).AddEffectState(EffectStateType.UnitLeader).AddEffectState(EffectStateType.KeyEndChest);
            context.spawner.SpawnPiece(context, spawnSettings, ref boardState);
            return false; // We returned an user-adjusted config.
        }
    }
}
