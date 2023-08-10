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

    public sealed class SmallLevelSequenceOverriddenRule : Rule, IConfigWritable<List<string>>, IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "The adventure's map order is adjusted";

        private static bool isRandomMaps;
        private static List<string> _globalAdjustments;
        private static List<string> _randomMaps = new List<string>
                    { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

        private static bool _isActivated;
        private readonly List<string> _adjustments;

        private readonly List<string> elvenFloors1 = new List<string>
                    { "ElvenFloor01", "ElvenFloor04", "ElvenFloor14", "ElvenFloor16", "ElvenFloor15" };

        private readonly List<string> elvenFloors2 = new List<string>
                    { "ElvenFloor03", "ElvenFloor07", "ElvenFloor13", "ElvenFloor17", "ElvenFloor15", "ElvenFloor14" };

        private readonly List<string> forestFloors1 = new List<string>
                    { "ForestFloor01", "ForestFloor07" };

        private readonly List<string> forestFloors2 = new List<string>
                    { "ForestFloor01", "ForestFloor02", "ForestFloor07", "ForestFloor08" };

        private readonly List<string> sewersFloors1 = new List<string>
                    { "SewersFloor01" };

        private readonly List<string> sewersFloors2 = new List<string>
                    { "SewersFloor01" };

        /// <summary>
        /// Initializes a new instance of the <see cref="SmallLevelSequenceOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">List of strings of LevelNames.</param>
        public SmallLevelSequenceOverriddenRule(List<string> adjustments)
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
            ReplaceExistingProperties(_globalAdjustments, elvenFloors1, forestFloors1, sewersFloors1, elvenFloors2, forestFloors2, sewersFloors2, gameContext);
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(
                    typeof(LevelSequenceConfiguration), "GetSequenceDefinition"),
                prefix: new HarmonyMethod(
                    typeof(SmallLevelSequenceOverriddenRule),
                    nameof(LevelSequenceConfiguration_GetSequenceDefinition_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(PlayAgainEventHandler), "AfterResponse"),
                prefix: new HarmonyMethod(
                    typeof(SmallLevelSequenceOverriddenRule),
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
            if (isRandomMaps)
            {
                if (newGameType == LevelSequence.GameType.Desert)
                {
                    _randomMaps[4] = "DesertBossFloor01";
                }
                else if (_randomMaps[4] == "DesertBossFloor01")
                {
                    _randomMaps[4] = "DesertFloor10";
                }
                else if (newGameType == LevelSequence.GameType.Town)
                {
                    _randomMaps[4] = "TownsBossFloor01";
                }
                else if (_randomMaps[4] == "TownsBossFloor01")
                {
                    _randomMaps[4] = "TownsFloor02";
                }

                Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                    _randomMaps.Prepend(originalSequence[0]).ToArray();
            }
            else
            {
                if (newGameType == LevelSequence.GameType.Desert)
                {
                    _globalAdjustments[4] = "DesertBossFloor01";
                }
                else if (newGameType == LevelSequence.GameType.Town)
                {
                    _globalAdjustments[4] = "TownsBossFloor01";
                }

                Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                    _globalAdjustments.Prepend(originalSequence[0]).ToArray();
            }

            eventQueue.SendEventRequest(new SerializableEventStartNewGame(gsmLevelSequence));
            return false;
        }

        /// <summary>
        /// Replaces LevelSequence levels with predefined list.
        /// </summary>
        /// <returns>List of previous LevelSequence levels that are now replaced.</returns>
        private static List<string> ReplaceExistingProperties(List<string> replacements, List<string> elvenFloors1, List<string> forestFloors1, List<string> sewersFloors1, List<string> elvenFloors2, List<string> forestFloors2, List<string> sewersFloors2, GameContext gameContext)
        {
            var gsmLevelSequence =
                Traverse.Create(gameContext.gameStateMachine).Field<LevelSequence>("levelSequence").Value;
            var originalSequence = Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value;

            if (replacements.Count == 5 && replacements[1].Contains("Shop") && replacements[3].Contains("Shop"))
            {
                isRandomMaps = false;
                if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
                {
                    replacements[4] = "DesertBossFloor01";
                }
                else if (gsmLevelSequence.gameType == LevelSequence.GameType.Town)
                {
                    replacements[4] = "TownsBossFloor01";
                }

                EssentialsMod.Logger.Warning("User configured specific level sequence loaded");
                EssentialsMod.Logger.Warning($"Map1: {replacements[0]} Shop1: {replacements[1]} Map2: {replacements[2]} Shop2: {replacements[3]} Map3: {replacements[4]}");
                Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                replacements.Prepend(originalSequence[0]).ToArray();
                return originalSequence.ToList();
            }
            else
            {
                isRandomMaps = true;
            }

            int rndLevel = Random.Range(1, 4);
            int rndMap1 = 0;
            int rndMap2 = 0;
            int rndMap3 = 0;

            switch (rndLevel)
            {
                case 1:
                    rndMap1 = Random.Range(1, 4);
                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1)
                    {
                        rndMap2 = Random.Range(1, 4);
                    }

                    rndMap3 = Random.Range(1, 4);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2)
                    {
                        rndMap3 = Random.Range(1, 4);
                    }

                    break;

                case 2:
                    rndMap1 = 2;
                    while (rndMap1 == 2)
                    {
                        rndMap1 = Random.Range(1, 4);
                    }

                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1)
                    {
                        rndMap2 = Random.Range(1, 4);
                    }

                    rndMap3 = Random.Range(1, 4);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2)
                    {
                        rndMap3 = Random.Range(1, 4);
                    }

                    break;

                case 3:
                    rndMap1 = 3;
                    while (rndMap1 == 3)
                    {
                        rndMap1 = Random.Range(1, 4);
                    }

                    rndMap2 = rndMap1;
                    while (rndMap2 == rndMap1)
                    {
                        rndMap2 = Random.Range(1, 4);
                    }

                    rndMap3 = Random.Range(1, 4);
                    while (rndMap3 == rndMap1 || rndMap3 == rndMap2)
                    {
                        rndMap3 = Random.Range(1, 4);
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
            }

            switch (rndMap3)
            {
                case 1:
                    newMap = Random.Range(0, elvenFloors1.Count);
                    _randomMaps[4] = elvenFloors1[newMap];
                    break;

                case 2:
                    newMap = Random.Range(0, forestFloors1.Count);
                    _randomMaps[4] = forestFloors1[newMap];
                    break;

                case 3:
                    newMap = Random.Range(0, sewersFloors1.Count);
                    _randomMaps[4] = sewersFloors1[newMap];
                    break;
            }

            if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
            {
                _randomMaps[4] = "DesertBossFloor01";
            }
            else if (gsmLevelSequence.gameType == LevelSequence.GameType.Town)
            {
                _randomMaps[4] = "TownsBossFloor01";
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
                    _randomMaps[3] = "TownsShopFloor";
                    break;
            }

            // EssentialsMod.Logger.Warning("Randomly generated level sequence loaded");
            EssentialsMod.Logger.Warning($"Map1: {_randomMaps[0]} Shop1: {_randomMaps[1]} Map2: {_randomMaps[2]} Shop2: {_randomMaps[3]} Map3: {_randomMaps[4]}");
            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                _randomMaps.Prepend(originalSequence[0]).ToArray();
            return originalSequence.ToList();
        }
    }
}
