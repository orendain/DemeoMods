namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using StatusEffectData = global::Types.StatusEffectData;

    public sealed class StatusEffectConfigRule : Rule, IConfigWritable<List<StatusEffectData>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Status Effect Durations are modified";

        private static bool _isActivated;
        private static List<StatusEffectData> _adjustments;

        public StatusEffectConfigRule(List<StatusEffectData> adjustments)
        {
            _adjustments = adjustments;
        }

        public List<StatusEffectData> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;

        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StatusEffectsConfig), "GetConfigFor"),
                prefix: new HarmonyMethod(
                    typeof(StatusEffectConfigRule),
                    nameof(StatusEffects_GetConfigFor_Prefix)));
        }

        private static bool StatusEffects_GetConfigFor_Prefix(ref EffectStateType type, ref StatusEffectData __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            for (int i = 0; i < _adjustments.Count; i++)
            {
                if (_adjustments[i].effectStateType == type)
                {
                    __result = _adjustments[i];
                    return false; // We returned an user-adjusted config.
                }
            }

            return true;
        }
    }
}
