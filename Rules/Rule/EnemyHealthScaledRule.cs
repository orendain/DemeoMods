namespace Rules.Rule
{
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class EnemyHealthScaledRule : RulesAPI.Rule, RulesAPI.IConfigWritable, RulesAPI.IPatchable
    {
        public override string Description => "Enemy health is scaled";

        private static float _multiplier;
        private static bool _isActivated;

        public EnemyHealthScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public static EnemyHealthScaledRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out float conf);
            return new EnemyHealthScaledRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_multiplier, EncodeOptions.NoTypeHints);
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(PieceConfig), "StartHealth"),
                postfix: new HarmonyMethod(
                    typeof(EnemyHealthScaledRule),
                    nameof(PieceConfig_StartHealth_Postfix)));
        }

        private static void PieceConfig_StartHealth_Postfix(PieceConfig __instance, ref int __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!__instance.HasPieceType(PieceType.Enemy))
            {
                return;
            }

            __result = (int)(__result * _multiplier);
        }
    }
}
