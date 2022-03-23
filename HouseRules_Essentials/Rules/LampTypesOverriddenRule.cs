namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.LayerCake;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class LampTypesOverriddenRule : Rule, IConfigWritable<LampTypesOverriddenRule.LampConfig>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Lamp types are overridden.";

        private static LampTypesOverriddenRule.LampConfig _globalAdjustments;
        private static bool _isActivated;

        private readonly LampTypesOverriddenRule.LampConfig _adjustments;

        public struct LampConfig
        {
            public List<BoardPieceId> Floor1Lamps;
            public List<BoardPieceId> Floor2Lamps;
            public List<BoardPieceId> Floor3Lamps;
        }

        public LampTypesOverriddenRule(LampTypesOverriddenRule.LampConfig adjustments)
        {
            _adjustments = adjustments;
        }

        public LampTypesOverriddenRule.LampConfig GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(ZoneSpawner), "GetLampTypes"),
                prefix: new HarmonyMethod(
                    typeof(LampTypesOverriddenRule),
                    nameof(ZoneSpawner_GetLampTypes_Prefix)));
        }

        private static bool ZoneSpawner_GetLampTypes_Prefix(ref BoardPieceId[] __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            var floorIndex = MotherTracker.motherTrackerData.floorIndex;
            EssentialsMod.Logger.Msg($"Lamps floor index {floorIndex}");

            if (floorIndex == 0)
            {
                __result = _globalAdjustments.Floor1Lamps.ToArray();
            }
            else if (floorIndex == 1)
            {
                __result = _globalAdjustments.Floor2Lamps.ToArray();
            }
            else
            {
                __result = _globalAdjustments.Floor3Lamps.ToArray();
            }

            return false; // We returned an user-adjusted config.
        }
    }
}
