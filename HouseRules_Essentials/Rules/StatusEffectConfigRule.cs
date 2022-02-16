namespace HouseRules.Essentials.Rules
{
    using System;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class StatusEffectConfigRule : Rule, IConfigWritable<global::Types.StatusEffectData[]>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Status Effect Durations are modified";

        private static bool _isActivated;
        private static global::Types.StatusEffectData[] _adjustments;

        public StatusEffectConfigRule(global::Types.StatusEffectData[] adjustments)
        {
            _adjustments = adjustments;
        }

        public global::Types.StatusEffectData[] GetConfigObject() => _adjustments;

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

        private static bool StatusEffects_GetConfigFor_Prefix(ref EffectStateType type, ref global::Types.StatusEffectData __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            var effectsConfig = Traverse.Create(typeof(StatusEffectsConfig)).Field<global::Types.StatusEffectData[]>("effectsConfig").Value;

            for (int i = 0; i < _adjustments.Length; i++)
            {
                if (_adjustments[i].effectStateType == type)
                {
                    __result = _adjustments[i];
                    return false; // We returned an user-adjusted config.
                }
            }
            for (int i = 0; i < effectsConfig.Length; i++)
            {
                if (effectsConfig[i].effectStateType == type)
                {
                    __result = effectsConfig[i];
                    return false; // We returned original result.
                }
            }

            throw new Exception("Could not find config for " + type);
        }
    }
}
