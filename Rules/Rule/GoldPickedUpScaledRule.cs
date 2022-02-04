namespace Rules.Rule
{
    using Boardgame.Data;
    using Boardgame.SerializableEvents;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class GoldPickedUpScaledRule : RulesAPI.Rule, RulesAPI.IConfigWritable
    {
        public override string Description => "Gold picked up is scaled";

        private static Config _config;
        private static bool _isActivated;

        private struct Config
        {
            public float Multiplier;
        }

        public GoldPickedUpScaledRule(float multiplier)
            : this(new Config { Multiplier = multiplier })
        {
        }

        private GoldPickedUpScaledRule(Config config)
        {
            _config = config;
        }

        public static GoldPickedUpScaledRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out Config conf);
            return new GoldPickedUpScaledRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_config, EncodeOptions.NoTypeHints);
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void OnPatch(Harmony harmony)
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

            __instance.goldAmount = (int)(__instance.goldAmount * _config.Multiplier);
        }
    }
}
