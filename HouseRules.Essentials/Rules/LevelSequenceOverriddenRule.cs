namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.SerializableEvents;
    using Boardgame.SerializableEvents.CustomEventHandlers;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class LevelSequenceOverriddenRule : Rule, IConfigWritable<List<string>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "The adventure's map order is adjusted";

        private static List<string> _globalAdjustments;
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

        protected override void OnActivate(Context context)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnPreGameCreated(Context context)
        {
            ReplaceExistingProperties(_globalAdjustments, context.GameContext);
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

            __result = gameContext.levelLoaderAndInitializer.GetLevelSequence().CurrentLevelIsLastLevel
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
                _globalAdjustments[4] = "DesertBossFloor01";
            }
            else if (newGameType == LevelSequence.GameType.Town)
            {
                _globalAdjustments[4] = "TownsBossFloor01";
            }

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                _globalAdjustments.Prepend(originalSequence[0]).ToArray();
            var gameState = gameContext.gameStateMachine.GetCurrentGameState();
            eventQueue.SendEventRequest(new SerializableEventStartNewGame(gsmLevelSequence, gameState));
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

            if (gsmLevelSequence.gameType == LevelSequence.GameType.Desert)
            {
                replacements[4] = "DesertBossFloor01";
            }
            else if (gsmLevelSequence.gameType == LevelSequence.GameType.Town)
            {
                replacements[4] = "TownsBossFloor01";
            }

            Traverse.Create(gsmLevelSequence).Field<string[]>("levels").Value =
                replacements.Prepend(originalSequence[0]).ToArray();
            return originalSequence.ToList();
        }
    }
}
