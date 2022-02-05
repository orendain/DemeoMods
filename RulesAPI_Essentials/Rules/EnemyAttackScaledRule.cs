namespace RulesAPI.Essentials.Rules
{
    using Boardgame;
    using DataKeys;
    using HarmonyLib;

    public sealed class EnemyAttackScaledRule : Rule, IConfigWritable<float>, IPatchable
    {
        public override string Description => "Enemy attack damage is scaled";

        private static float _multiplier;
        private static bool _isActivated;

        public EnemyAttackScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

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
