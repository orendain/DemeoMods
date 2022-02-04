namespace Rules.Rule
{
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class EnemyAttackScaledRule : RulesAPI.Rule, RulesAPI.IConfigWritable, RulesAPI.IPatchable
    {
        public override string Description => "Enemy attack damage is scaled";

        private static float _multiplier;
        private static bool _isActivated;

        public EnemyAttackScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public static EnemyAttackScaledRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out float conf);
            return new EnemyAttackScaledRule(conf);
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
                original: AccessTools.PropertyGetter(typeof(PieceConfig), "AttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(EnemyAttackScaledRule),
                    nameof(PieceConfig_AttackDamage_Postfix)));
        }

        private static void PieceConfig_AttackDamage_Postfix(PieceConfig __instance, ref int __result)
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
