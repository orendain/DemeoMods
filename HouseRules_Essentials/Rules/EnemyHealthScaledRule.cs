namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class EnemyHealthScaledRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy health is scaled";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public EnemyHealthScaledRule(float multiplier)
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
                original: AccessTools.PropertyGetter(typeof(PieceConfigData), "StartHealth"),
                postfix: new HarmonyMethod(
                    typeof(EnemyHealthScaledRule),
                    nameof(PieceConfig_StartHealth_Postfix)));
        }

        private static void PieceConfig_StartHealth_Postfix(PieceConfigData __instance, ref int __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!__instance.HasPieceType(PieceType.Enemy))
            {
                return;
            }

            if (__instance.PowerIndex < 41)
            {
                float range = Random.Range(0.75f, 1.0f);
                __result = (int)((__result * _globalMultiplier) * range);
            }
            else
            {
                __result = (int)(__result * _globalMultiplier);
            }
        }
    }
}
