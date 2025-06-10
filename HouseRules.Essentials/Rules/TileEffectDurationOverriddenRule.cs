namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class TileEffectDurationOverriddenRule : Rule, IConfigWritable<Dictionary<TileEffect, int>>,
        IPatchable, IMultiplayerSafe
    {
        public override string Description => "Tile effect durations are adjusted";

        private static Dictionary<TileEffect, int> _globalAdjustments;
        private static bool _isActivated;

        private readonly Dictionary<TileEffect, int> _adjustments;

        public TileEffectDurationOverriddenRule(Dictionary<TileEffect, int> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<TileEffect, int> GetConfigObject() => _adjustments;

        protected override void OnActivate(Context context)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(TileEffectState), "GetLayerExpireTime"),
                prefix: new HarmonyMethod(
                    typeof(TileEffectDurationOverriddenRule),
                    nameof(TileEffectState_GetLayerExpireTime_Prefix)));
        }

        private static bool TileEffectState_GetLayerExpireTime_Prefix(TileEffect type, ref int __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (!_globalAdjustments.ContainsKey(type))
            {
                return true;
            }

            __result = _globalAdjustments[type];
            return false; // We returned an user-adjusted config.
        }
    }
}
