﻿namespace Rules.Rule
{
    using Boardgame.Data;
    using Boardgame.SerializableEvents;
    using HarmonyLib;

    public sealed class GoldPickedUpMultipliedRule : RulesAPI.Rule
    {
        public override string Description => "Gold picked up is multiplied";

        private static double _multiplier;
        private static bool _isActivated;

        public GoldPickedUpMultipliedRule(double multiplier)
        {
            _multiplier = multiplier;
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void OnPatch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(SerializableEventPickup),
                    new[] { typeof(int), typeof(IntPoint2D), typeof(bool) }),
                postfix: new HarmonyMethod(typeof(GoldPickedUpMultipliedRule), nameof(SerializableEventPickup_Constructor_Postfix)));
        }

        private static void SerializableEventPickup_Constructor_Postfix(ref SerializableEventPickup __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            __instance.goldAmount = (int)(__instance.goldAmount * _multiplier);
        }
    }
}
