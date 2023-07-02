namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class ElvenQueenBuffsRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "The Elven Queen fight now has phases";

        private static bool _globalAdjustments;
        private static bool _isActivated;
        private static int phase = 0;

        private readonly bool _adjustments;

        internal static bool IsSuperBuffed => ElvenQueenSuperBuffRule.SuperBuff;

        public ElvenQueenBuffsRule(bool adjustments)
        {
            _adjustments = adjustments;
        }

        public bool GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Ability), "GenerateAttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(ElvenQueenBuffsRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source)
        {
            if (!_isActivated || source.boardPieceId != BoardPieceId.ElvenQueen)
            {
                return;
            }

            // Elven Queen now has 'phases' based on her current health to make her more challenging
            int nextPhase;
            int low = 1;
            int high = 6;
            if (source.GetHealth() >= (source.GetMaxHealth() / 2))
            {
                nextPhase = Random.Range(1, 6);
            }
            else if (source.GetHealth() < (source.GetMaxHealth() / 3))
            {
                if (IsSuperBuffed)
                {
                    return;
                }

                nextPhase = Random.Range(2, 6);
            }
            else
            {
                low = 2;
                high = 6;
                nextPhase = Random.Range(2, 6);
            }

            while (nextPhase == phase)
            {
                nextPhase = Random.Range(low, high);
            }

            phase = nextPhase;
            switch (nextPhase)
            {
                case 1:
                    source.EnableEffectState(EffectStateType.Deflect);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Deflect, 1);
                    break;
                case 2:
                    source.EnableEffectState(EffectStateType.MagicShield);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.MagicShield, 1);
                    break;
                case 3:
                    source.EnableEffectState(EffectStateType.FireImmunity);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 1);
                    break;
                case 4:
                    source.EnableEffectState(EffectStateType.Recovery);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Recovery, 2);
                    break;
                case 5:
                    source.EnableEffectState(EffectStateType.Courageous);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Courageous, 1);
                    break;
            }
        }
    }
}
