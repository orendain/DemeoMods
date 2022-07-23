﻿namespace HouseRules.Essentials.Rules
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
        private static bool _isActivated;
        private readonly List<string> _adjustments;

        private readonly List<string> elvenFloors1 = new List<string>
                    { "ElvenFloor14", "ElvenFloor16", "ElvenFloor17", "ElvenFloor13" };

        private readonly List<string> elvenFloors2 = new List<string>
                    { "ElvenFloor08", "ElvenFloor09", "ElvenFloor12", "ElvenFloor02" };

        private readonly List<string> forestFloors1 = new List<string>
                    { "ForestFloor01", "ForestFloor02", "ForestFloor07", "ForestFloor08" };

        private readonly List<string> forestFloors2 = new List<string>
                    { "ForestFloor03", "ForestFloor06", "ForestFloor09", "ForestFloor05" };

        private readonly List<string> sewersFloors1 = new List<string>
                    { "SewersFloor01", "SewersFloor07", "SewersFloor11", "SewersFloor09" };

        private readonly List<string> sewersFloors2 = new List<string>
                    { "SewersFloor10", "SewersFloor12", "SewersFloor11", "SewersFloor08" };

        private readonly List<string> desertFloors1 = new List<string>
                    { "DesertFloor10", "DesertFloor06" };

        private readonly List<string> desertFloors2 = new List<string>
                    { "DesertFloor02", "DesertFloor08", "DesertFloor09" };

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
            ReplaceExistingProperties(_globalAdjustments, elvenFloors1, forestFloors1, sewersFloors1, desertFloors1, elvenFloors2, forestFloors2, sewersFloors2, desertFloors2, gameContext);
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

            if (gameType == LevelSequence.GameType.Desert)
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
                for (var i = 0; i < _globalAdjustments.Count; i++)
                {
                    if (i == _globalAdjustments.Count - 1)
                    {
                        _globalAdjustments[i] = "DesertBossFloor01";
                    }
                }
            }

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                _globalAdjustments.Prepend(originalSequence[0]).ToArray();
            eventQueue.SendEventRequest(new SerializableEventStartNewGame(gsmLevelSequence));
            return false;
        }

        /// <summary>
        /// Replaces LevelSequence levels with predefined list.
        /// </summary>
        /// <returns>List of previous LevelSequence levels that are now replaced.</returns>
        private static List<string> ReplaceExistingProperties(List<string> replacements, List<string> elvenFloors1, List<string> forestFloors1, List<string> sewersFloors1, List<string> desertFloors1, List<string> elvenFloors2, List<string> forestFloors2, List<string> sewersFloors2, List<string> desertFloors2, GameContext gameContext)
        {
            var gsmLevelSequence =
                Traverse.Create(gameContext.gameStateMachine).Field<LevelSequence>("levelSequence").Value;
            var originalSequence = Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value;

            int rndLevel = Random.Range(1, 5);
            int rndMap1;
            int rndMap2;
            int rndMap3;
            int newMap;

            EssentialsMod.Logger.Warning($"Omap1: {replacements[0]} Oshop1: {replacements[1]} Omap2: {replacements[2]} Oshop2: {replacements[3]} Omap3: {replacements[4]}");
            if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
            {
                rndLevel = 4;
            }

            if (rndLevel == 1)
            {
                rndMap1 = Random.Range(2, 5);
                rndMap2 = rndMap1;
                while (rndMap2 == rndMap1)
                {
                    rndMap2 = Random.Range(2, 5);
                }

                rndMap3 = 2;
                while (rndMap3 == rndMap1 || rndMap3 == rndMap2)
                {
                    rndMap3++;
                }
            }
            else if (rndLevel == 2)
            {
                rndMap1 = 2;
                while (rndMap1 == 2)
                {
                    rndMap1 = Random.Range(1, 5);
                }

                rndMap2 = rndMap1;
                while (rndMap2 == rndMap1 || rndMap2 == 2)
                {
                    rndMap2 = Random.Range(1, 5);
                }

                rndMap3 = 1;
                while (rndMap3 == rndMap1 || rndMap3 == rndMap2 || rndMap3 == 2)
                {
                    rndMap3++;
                }
            }
            else if (rndLevel == 3)
            {
                rndMap1 = 3;
                while (rndMap1 == 3)
                {
                    rndMap1 = Random.Range(1, 5);
                }

                rndMap2 = rndMap1;
                while (rndMap2 == rndMap1 || rndMap2 == 3)
                {
                    rndMap2 = Random.Range(1, 5);
                }

                rndMap3 = 1;
                while (rndMap3 == rndMap1 || rndMap3 == rndMap2 || rndMap3 == 3)
                {
                    rndMap3++;
                }
            }
            else
            {
                rndMap1 = Random.Range(1, 4);
                rndMap2 = rndMap1;
                while (rndMap2 == rndMap1)
                {
                    rndMap2 = Random.Range(1, 4);
                }

                rndMap3 = 1;
                while (rndMap3 == rndMap1 || rndMap3 == rndMap2 || rndMap3 == 4)
                {
                    rndMap3++;
                }
            }

            if (rndMap1 == 1)
            {
                newMap = Random.Range(0, elvenFloors1.Count);
                replacements[0] = elvenFloors1[newMap];
            }
            else if (rndMap1 == 2)
            {
                newMap = Random.Range(0, forestFloors1.Count);
                replacements[0] = forestFloors1[newMap];
            }
            else if (rndMap1 == 3)
            {
                newMap = Random.Range(0, sewersFloors1.Count);
                replacements[0] = sewersFloors1[newMap];
            }
            else
            {
                newMap = Random.Range(0, desertFloors1.Count);
                replacements[0] = desertFloors1[newMap];
            }

            if (rndMap2 == 1)
            {
                newMap = Random.Range(0, elvenFloors2.Count);
                replacements[2] = elvenFloors2[newMap];
            }
            else if (rndMap2 == 2)
            {
                newMap = Random.Range(0, forestFloors2.Count);
                replacements[2] = forestFloors2[newMap];
            }
            else if (rndMap2 == 3)
            {
                newMap = Random.Range(0, sewersFloors2.Count);
                replacements[2] = sewersFloors2[newMap];
            }
            else
            {
                newMap = Random.Range(0, desertFloors2.Count);
                replacements[2] = desertFloors2[newMap];
            }

            if (rndMap3 == 1)
            {
                newMap = Random.Range(0, elvenFloors1.Count);
                replacements[4] = elvenFloors1[newMap];
            }
            else if (rndMap3 == 2)
            {
                newMap = Random.Range(0, forestFloors1.Count);
                replacements[4] = forestFloors1[newMap];
            }
            else if (rndMap3 == 3)
            {
                newMap = Random.Range(0, sewersFloors1.Count);
                replacements[4] = sewersFloors1[newMap];
            }
            else
            {
                newMap = Random.Range(0, forestFloors1.Count);
                replacements[4] = desertFloors1[newMap];
            }

            if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
            {
                replacements[4] = "DesertBossFloor01";
            }

            if (replacements[2].Contains("Elven"))
            {
                replacements[1] = "ShopFloor02";
            }
            else if (replacements[2].Contains("Forest"))
            {
                replacements[1] = "ForestShopFloor";
            }
            else if (replacements[2].Contains("Sewer"))
            {
                replacements[1] = "SewersShopFloor";
            }
            else if (replacements[2].Contains("Desert"))
            {
                replacements[1] = "DesertShopFloor";
            }

            if (replacements[4].Contains("Elven"))
            {
                replacements[3] = "ShopFloor02";
            }
            else if (replacements[4].Contains("Forest"))
            {
                replacements[3] = "ForestShopFloor";
            }
            else if (replacements[4].Contains("Sewer"))
            {
                replacements[3] = "SewersShopFloor";
            }
            else if (replacements[4].Contains("Desert"))
            {
                replacements[3] = "DesertShopFloor";
            }

            EssentialsMod.Logger.Warning($"Nmap1: {replacements[0]} Nshop1: {replacements[1]} Nmap2: {replacements[2]} Nshop2: {replacements[3]} Nmap3: {replacements[4]}");

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                replacements.Prepend(originalSequence[0]).ToArray();
            return originalSequence.ToList();
        }
    }
}
