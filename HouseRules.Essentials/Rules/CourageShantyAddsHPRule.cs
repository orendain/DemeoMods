namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using UnityEngine;

    public sealed class CourageShantyAddsHpRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Courage Shanty can also give a small heal";

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

            var piece = target.piece;
            if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Revolutions"))
            {
                if (Random.Range(1, 101) > 66)
                {
                    if (piece.GetHealth() < piece.GetMaxHealth())
                    {
                        piece.DisableEffectState(EffectStateType.Heal);
                        piece.EnableEffectState(EffectStateType.Heal);
                        piece.effectSink.Heal(_globalAdjustments);
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Downed);
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Stunned);
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Frozen);
                    }

                    HR.ScheduleBoardSync();
                }
            }
            else
            {
                if (piece.GetHealth() < piece.GetMaxHealth())
                {
                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal);
                    piece.effectSink.Heal(_globalAdjustments);
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Downed);
                }

                HR.ScheduleBoardSync();
            }
        }
    }
}
