namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class CourageShantyAddsHpRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Courage Shanty also adds HP.";

        private static int _globalAdjustments;
        private static bool _isActivated;

        private readonly int _adjustments;

        public CourageShantyAddsHpRule(int adjustments)
        {
            _adjustments = adjustments;
        }

        public int GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StrengthenCourage), "UpdateEffectsOnTarget"),
                postfix: new HarmonyMethod(
                    typeof(CourageShantyAddsHpRule),
                    nameof(StrengthenCourage_UpdateEffectsOnTarget_Postfix)));
        }

        private static void StrengthenCourage_UpdateEffectsOnTarget_Postfix(Target target)
        {
            if (!_isActivated)
            {
                return;
            }

            if (target.piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69)
            {
                if (Random.Range(1, 101) > 66)
                {
                    target.piece.effectSink.Heal(_globalAdjustments);
                    target.piece.effectSink.RemoveStatusEffect(EffectStateType.Downed);
                    target.piece.AnimateWobble();
                    HR.ScheduleBoardSync();
                }
            }
            else
            {
                target.piece.effectSink.Heal(_globalAdjustments);
                HR.ScheduleBoardSync();
            }
        }
    }
}
