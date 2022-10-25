namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

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

            if (__result > 2)
            {
                if (__instance.PowerIndex < 25)
                {
                    int range = Random.Range(-1, 2);
                    __result = (int)(__result * _globalMultiplier) + range;
                }
                else if (__instance.PowerIndex > 24 && __instance.PowerIndex < 41)
                {
                    int range = Random.Range(0, 2);
                    __result = (int)(__result * _globalMultiplier) - range;
                }
                else
                {
                    __result = (int)(__result * _globalMultiplier);
                }
            }
            else
            {
                __result = (int)(__result * _globalMultiplier);
            }
        }
    }
}
