namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class EnemyAttackScaledRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy attack damage is scaled";

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public EnemyAttackScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalMultiplier = _multiplier;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(PieceConfigData), "AttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(EnemyAttackScaledRule),
                    nameof(PieceConfig_AttackDamage_Postfix)));
        }

        private static void PieceConfig_AttackDamage_Postfix(PieceConfigData __instance, ref int __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (__instance.HasPieceType(PieceType.Player) || __instance.HasPieceType(PieceType.Bot))
            {
                return;
            }

            __result = (int)(__result * _globalMultiplier);
        }
    }
}
