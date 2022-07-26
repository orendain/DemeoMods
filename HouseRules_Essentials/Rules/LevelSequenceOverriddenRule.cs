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

    public sealed class LevelSequenceOverriddenRule : Rule, IConfigWritable<List<string>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "LevelSequence is overridden";

        private static List<string> _globalAdjustments;
        private static List<string> _randomMaps = new List<string>
                    { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

        private static bool _fixHydra = false;
        private static bool _isActivated;
        private readonly List<string> _adjustments;

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
            ReplaceExistingProperties(_globalAdjustments, gameContext);
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
                    EssentialsMod.Logger.Warning("Fix the Hydra!");
                    _randomMaps[4] = "DesertBossFloor01";
                }
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
        private static List<string> ReplaceExistingProperties(List<string> replacements, GameContext gameContext)
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

            EssentialsMod.Logger.Warning("Loading randomized level sequence");
            int newMap;
            int x = replacements.Count;
            if (replacements[x - 1].ToLower() == "fixhydra")
            {
                x--;
                _fixHydra = true;
                EssentialsMod.Logger.Warning("Fix the Hydra!");
            }

            newMap = Random.Range(0, x);
            _randomMaps[0] = replacements[newMap];

            while (replacements[newMap] == _randomMaps[0])
            {
                newMap = Random.Range(0, x);
            }

            _randomMaps[2] = replacements[newMap];
            while (replacements[newMap] == _randomMaps[0] || replacements[newMap] == _randomMaps[2])
            {
                newMap = Random.Range(0, x);
            }

            if (_fixHydra)
            {
                replacements[4] = "DesertBossFloor01";
            }
            else
            {
                _randomMaps[4] = replacements[newMap];
            }

            switch (_randomMaps[2].Substring(0, 5))
            {
                case "Elven":
                    _randomMaps[1] = "ShopFloor02";
                    break;

                case "Fores":
                    _randomMaps[1] = "ForestShopFloor";
                    break;

                case "Sewer":
                    _randomMaps[1] = "SewersShopFloor";
                    break;

                case "Deser":
                    _randomMaps[1] = "DesertShopFloor";
                    break;
            }

            switch (_randomMaps[4].Substring(0, 5))
            {
                case "Elven":
                    _randomMaps[3] = "ShopFloor02";
                    break;

                case "Fores":
                    _randomMaps[3] = "ForestShopFloor";
                    break;

                case "Sewer":
                    _randomMaps[3] = "SewersShopFloor";
                    break;

                case "Deser":
                    _randomMaps[3] = "DesertShopFloor";
                    break;
            }

            EssentialsMod.Logger.Warning($"Map1: {_randomMaps[0]} Shop1: {_randomMaps[1]} Map2: {_randomMaps[2]} Shop2: {_randomMaps[3]} Map3: {_randomMaps[4]}");

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                _randomMaps.Prepend(originalSequence[0]).ToArray();
            return originalSequence.ToList();
        }
    }
}
