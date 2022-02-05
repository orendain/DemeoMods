namespace RulesAPI.Essentials.Rules
{
    using Boardgame.Data;
    using Boardgame.SerializableEvents;
    using HarmonyLib;

    public sealed class GoldPickedUpScaledRule : Rule, IConfigWritable<float>, IPatchable
    {
        public override string Description => "Gold picked up is scaled";

        private static float _multiplier;
        private static bool _isActivated;

        public GoldPickedUpScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(SerializableEventPickup),
                    new[] { typeof(int), typeof(IntPoint2D), typeof(bool) }),
                postfix: new HarmonyMethod(typeof(GoldPickedUpScaledRule), nameof(SerializableEventPickup_Constructor_Postfix)));
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
