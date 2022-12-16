namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.SerializableEvents;
    using Boardgame.SerializableEvents.CustomEventHandlers;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class LevelSequenceOverriddenRule : Rule, IConfigWritable<List<string>>, IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "LevelSequence is overridden";

        private static List<string> _globalAdjustments;
        private static List<string> _randomMaps = new List<string>
                    { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

        private static bool _fixHydra;
        private static bool _fixKing;
        private static bool _isActivated;
        private readonly List<string> _adjustments;

        private readonly List<string> elvenFloors1 = new List<string>
                    { "ElvenFloor01", "ElvenFloor04", "ElvenFloor13", "ElvenFloor14", "ElvenFloor16", "ElvenFloor17" };

        private readonly List<string> elvenFloors2 = new List<string>
                    { "ElvenFloor02", "ElvenFloor03", "ElvenFloor06", "ElvenFloor07", "ElvenFloor08", "ElvenFloor10", "ElvenFloor11" };

        private readonly List<string> forestFloors1 = new List<string>
                    { "ForestFloor01", "ForestFloor02", "ForestFloor07", "ForestFloor08" };

        private readonly List<string> forestFloors2 = new List<string>
                    { "ForestFloor03", "ForestFloor06", "ForestFloor07", "ForestFloor09" };

        private readonly List<string> sewersFloors1 = new List<string>
                    { "SewersFloor01", "SewersFloor07", "SewersFloor09", "SewersFloor10" };

        private readonly List<string> sewersFloors2 = new List<string>
                    { "SewersFloor08", "SewersFloor09", "SewersFloor10", "SewersFloor11", "SewersFloor12" };

        private readonly List<string> desertFloors1 = new List<string>
                    { "DesertFloor10", "DesertFloor06" };

        private readonly List<string> desertFloors2 = new List<string>
                    { "DesertFloor02", "DesertFloor08", "DesertFloor09", "DesertFloor06", "DesertFloor10" };

        private readonly List<string> townsFloors1 = new List<string>
                    { "TownsFloor04", "TownsFloor05", "TownsFloor06" };

        private readonly List<string> townsFloors2 = new List<string>
                    { "TownsFloor01", "TownsFloor02", "TownsFloor03", "TownsFloor08" };

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelSequenceOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">List of strings of LevelNames.</param>
        public LevelSequenceOverriddenRule(List<string> adjustments)
        {
            _adjustments = adjustments;
        }

        public List<string> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            ReplaceExistingProperties(_globalAdjustments, elvenFloors1, forestFloors1, sewersFloors1, desertFloors1, townsFloors1, elvenFloors2, forestFloors2, sewersFloors2, desertFloors2, townsFloors2, gameContext);
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(
                    typeof(LevelSequenceConfiguration), "GetSequenceDefinition"),
                prefix: new HarmonyMethod(
                    typeof(LevelSequenceOverriddenRule),
                    nameof(LevelSequenceConfiguration_GetSequenceDefinition_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(PlayAgainEventHandler), "AfterResponse"),
                prefix: new HarmonyMethod(
                    typeof(LevelSequenceOverriddenRule),
                    nameof(PlayAgainEventHandler_AfterResponse_Prefix)));
        }

        /// <remarks>
        /// Sets a safe sequence definition even if the active game type does not have one that extends to the current level.
        /// </remarks>
        private static bool LevelSequenceConfiguration_GetSequenceDefinition_Prefix(
            ref SequenceDefinition __result,
            int index,
            LevelSequence.GameType gameType)
        {
            if (!_isActivated)
            {
                return true;
            }

            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var sequenceDefinitions =
                gameContext.levelSequenceConfiguration.sequenceDefinitions.GetSequenceFromId(gameType, out _);

            if (index >= 0 && index < sequenceDefinitions.Length)
            {
                return true;
            }

            __result = gameContext.levelManager.GetLevelSequence().CurrentLevelIsLastLevel
                ? sequenceDefinitions[sequenceDefinitions.Length - 1]
                : sequenceDefinitions[sequenceDefinitions.Length - 3];

            return false;
        }

        /// <remarks>
        /// Overrides the level sequence used for a restarted game with the fresh copy of the current overriden one.
        /// </remarks>
        private static bool PlayAgainEventHandler_AfterResponse_Prefix(
            PlayAgainEventHandler __instance,
            SerializableEventQueue eventQueue)
        {
            if (!_isActivated)
            {
                return true;
            }

            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            var newGameType =
                Traverse.Create(__instance).Field<PostGameControllerBase>("postGameController").Value.gameType;

            var gsmLevelSequence = gameContext.levelSequenceConfiguration.GetNewLevelSequence(-1, newGameType, LevelSequence.ControlType.OneHero);
            var originalSequence = Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value;

            if (newGameType == LevelSequence.GameType.Desert)
            {
                if (_fixHydra)
                {
                    _randomMaps[4] = "DesertBossFloor01";
                }
            }
            else if (_randomMaps[4] == "DesertBossFloor01")
            {
                _randomMaps[4] = "DesertFloor10";
            }

            if (newGameType == LevelSequence.GameType.Town)
            {
                if (_fixKing)
                {
                    _randomMaps[4] = "TownsBossFloor01";
                }
            }
            else if (_randomMaps[4] == "TownsBossFloor01")
            {
                _randomMaps[4] = "TownsFloor02";
            }

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                _randomMaps.Prepend(originalSequence[0]).ToArray();
            eventQueue.SendEventRequest(new SerializableEventStartNewGame(gsmLevelSequence));
            return false;
        }

        /// <summary>
        /// Replaces LevelSequence levels with predefined list.
        /// </summary>
        /// <returns>List of previous LevelSequence levels that are now replaced.</returns>
        private static List<string> ReplaceExistingProperties(List<string> replacements, List<string> elvenFloors1, List<string> forestFloors1, List<string> sewersFloors1, List<string> desertFloors1, List<string> townsFloors1, List<string> elvenFloors2, List<string> forestFloors2, List<string> sewersFloors2, List<string> desertFloors2, List<string> townsFloors2, GameContext gameContext)
        {
            var gsmLevelSequence =
                Traverse.Create(gameContext.gameStateMachine).Field<LevelSequence>("levelSequence").Value;
            var originalSequence = Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value;

            if (replacements.Count == 5 || replacements[1].Contains("Shop"))
            {
                EssentialsMod.Logger.Warning("User configured specific level sequence loaded");
                Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                replacements.Prepend(originalSequence[0]).ToArray();
                return originalSequence.ToList();
            }

            int rndLevel = Random.Range(1, 6);
            int rndMap1 = 0;
            int rndMap2 = 0;
            int rndMap3 = 0;

            if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
            {
                rndLevel = 4;
                if (replacements[replacements.Count - 2] == "fixhydra")
                {
                    _fixHydra = true;
                }
            }
            else if (gsmLevelSequence.gameType == LevelSequence.GameType.Town)
            {
                rndLevel = 5;
                if (replacements[replacements.Count - 1] == "fixking")
                {
                    _fixKing = true;
                }
            }

            switch (rndLevel)
            {
                case 1:
                    rndMap1 = Random.Range(2, 6);
                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1)
                    {
                        rndMap2 = Random.Range(2, 6);
                    }

                    rndMap3 = Random.Range(2, 6);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2)
                    {
                        rndMap3 = Random.Range(2, 6);
                    }

                    break;

                case 2:
                    rndMap1 = 2;
                    while (rndMap1 == 2)
                    {
                        rndMap1 = Random.Range(1, 6);
                    }

                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1 || rndMap2 == 2)
                    {
                        rndMap2 = Random.Range(1, 6);
                    }

                    rndMap3 = Random.Range(1, 6);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2 || rndMap3 == 2)
                    {
                        rndMap3 = Random.Range(1, 6);
                    }

                    break;

                case 3:
                    rndMap1 = 3;
                    while (rndMap1 == 3)
                    {
                        rndMap1 = Random.Range(1, 6);
                    }

                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1 || rndMap2 == 3)
                    {
                        rndMap2 = Random.Range(1, 6);
                    }

                    rndMap3 = Random.Range(1, 6);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2 || rndMap3 == 3)
                    {
                        rndMap3 = Random.Range(1, 6);
                    }

                    break;
                case 4:
                    rndMap1 = 4;
                    while (rndMap1 == 4)
                    {
                        rndMap1 = Random.Range(1, 6);
                    }

                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1 || rndMap2 == 4)
                    {
                        rndMap2 = Random.Range(1, 6);
                    }

                    rndMap3 = Random.Range(1, 6);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2 || rndMap3 == 4)
                    {
                        rndMap3 = Random.Range(1, 6);
                    }

                    break;
                case 5:
                    rndMap1 = Random.Range(1, 5);
                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1)
                    {
                        rndMap2 = Random.Range(1, 5);
                    }

                    rndMap3 = Random.Range(1, 5);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2)
                    {
                        rndMap3 = Random.Range(1, 5);
                    }

                    break;
            }

            int newMap;
            switch (rndMap1)
            {
                case 1:
                    newMap = Random.Range(0, elvenFloors1.Count);
                    _randomMaps[0] = elvenFloors1[newMap];
                    break;

                case 2:
                    newMap = Random.Range(0, forestFloors1.Count);
                    _randomMaps[0] = forestFloors1[newMap];
                    break;

                case 3:
                    newMap = Random.Range(0, sewersFloors1.Count);
                    _randomMaps[0] = sewersFloors1[newMap];
                    break;

                case 4:
                    newMap = Random.Range(0, desertFloors1.Count);
                    _randomMaps[0] = desertFloors1[newMap];
                    break;
                case 5:
                    newMap = Random.Range(0, townsFloors1.Count);
                    _randomMaps[0] = townsFloors1[newMap];
                    break;
            }

            switch (rndMap2)
            {
                case 1:
                    newMap = Random.Range(0, elvenFloors2.Count);
                    _randomMaps[2] = elvenFloors2[newMap];
                    break;

                case 2:
                    newMap = Random.Range(0, forestFloors2.Count);
                    _randomMaps[2] = forestFloors2[newMap];
                    break;

                case 3:
                    newMap = Random.Range(0, sewersFloors2.Count);
                    _randomMaps[2] = sewersFloors2[newMap];
                    break;

                case 4:
                    newMap = Random.Range(0, desertFloors2.Count);
                    _randomMaps[2] = desertFloors2[newMap];
                    break;
                case 5:
                    newMap = Random.Range(0, townsFloors2.Count);
                    _randomMaps[2] = townsFloors2[newMap];
                    break;
            }

            switch (rndMap3)
            {
                case 1:
                    newMap = Random.Range(0, elvenFloors1.Count + 1);
                    if (newMap == elvenFloors1.Count)
                    {
                        _randomMaps[4] = "ElvenFloor09";
                    }
                    else
                    {
                        _randomMaps[4] = elvenFloors1[newMap];
                    }

                    break;

                case 2:
                    newMap = Random.Range(0, forestFloors1.Count);
                    _randomMaps[4] = forestFloors1[newMap];
                    break;

                case 3:
                    newMap = Random.Range(0, sewersFloors1.Count);
                    _randomMaps[4] = sewersFloors1[newMap];
                    break;

                case 4:
                    newMap = Random.Range(0, desertFloors1.Count);
                    _randomMaps[4] = desertFloors1[newMap];
                    break;
                case 5:
                    newMap = Random.Range(0, townsFloors1.Count);
                    _randomMaps[4] = townsFloors1[newMap];
                    break;
            }

            if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
            {
                if (_fixHydra)
                {
                    _randomMaps[4] = "DesertBossFloor01";
                }
            }
            else if (gsmLevelSequence.gameType == LevelSequence.GameType.Town)
            {
                if (_fixKing)
                {
                    _randomMaps[4] = "TownsBossFloor01";
                }
            }

            switch (_randomMaps[2].Substring(0, 4))
            {
                case "Elve":
                    _randomMaps[1] = "ShopFloor02";
                    break;

                case "Fore":
                    _randomMaps[1] = "ForestShopFloor";
                    break;

                case "Sewe":
                    _randomMaps[1] = "SewersShopFloor";
                    break;

                case "Dese":
                    _randomMaps[1] = "DesertShopFloor";
                    break;
                case "Town":
                    _randomMaps[1] = "TownShopFloor";
                    break;
            }

            switch (_randomMaps[4].Substring(0, 4))
            {
                case "Elve":
                    _randomMaps[3] = "ShopFloor02";
                    break;

                case "Fore":
                    _randomMaps[3] = "ForestShopFloor";
                    break;

                case "Sewe":
                    _randomMaps[3] = "SewersShopFloor";
                    break;

                case "Dese":
                    _randomMaps[3] = "DesertShopFloor";
                    break;
                case "Town":
                    _randomMaps[3] = "TownShopFloor";
                    break;
            }

            EssentialsMod.Logger.Msg($"Original: {originalSequence[0]} {originalSequence[1]} {originalSequence[2]} {originalSequence[4]} {originalSequence[5]}");
            EssentialsMod.Logger.Warning($"Map1: {_randomMaps[0]} Shop1: {_randomMaps[1]} Map2: {_randomMaps[2]} Shop2: {_randomMaps[3]} Map3: {_randomMaps[4]}");

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                _randomMaps.Prepend(originalSequence[0]).ToArray();
            return originalSequence.ToList();
        }
    }
}
