namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.LayerCake;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class LampTypesOverriddenRule : Rule, IConfigWritable<Dictionary<int, List<BoardPieceId>>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Lamp spawn types and variety are adjusted";

        private static Dictionary<int, List<BoardPieceId>> _globalAdjustments;
        private static bool _isActivated;

        private readonly Dictionary<int, List<BoardPieceId>> _adjustments;

        public LampTypesOverriddenRule(Dictionary<int, List<BoardPieceId>> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<int, List<BoardPieceId>> GetConfigObject() => _adjustments;

        protected override void OnActivate(Context context)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(Context context) => _isActivated = false;

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

            var floorIndex = MotherTracker.motherTrackerData.floorIndex + 1;
            if (!_globalAdjustments.ContainsKey(floorIndex))
            {
                return true;
            }

            __result = _globalAdjustments[floorIndex].ToArray();
            return false; // We returned an user-adjusted config.
        }
    }
}
