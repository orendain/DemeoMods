namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;
    using UnityEngine;

    public sealed class HeroesEnemyBuffsRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemies now have buff phases";

        private static bool _globalAdjustments;
        private static bool _isActivated;
        private static int phase = 0;

        private readonly bool _adjustments;

        internal static bool IsSuperBuffed => ElvenQueenSuperBuffRule.SuperBuff;

        public HeroesEnemyBuffsRule(bool adjustments)
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
                    typeof(HeroesEnemyBuffsRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source)
        {
            // if this is the ElvenQueen.
            if (_isActivated && source.boardPieceId == BoardPieceId.ElvenQueen)
            {
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

            // else if this is the WizardBoss.
            else if (_isActivated && source.boardPieceId == BoardPieceId.WizardBoss)
            {
                // Wizard Boss now has 'phases' based on his current health to make her more challenging
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
                        source.EnableEffectState(EffectStateType.Antidote);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.Antidote, 5);
                        break;
                    case 2:
                        source.EnableEffectState(EffectStateType.MagicShield);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.MagicShield, 3);
                        break;
                    case 3:
                        source.EnableEffectState(EffectStateType.FireImmunity);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 4);
                        break;
                    case 4:
                        source.EnableEffectState(EffectStateType.IceImmunity);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.IceImmunity, 4);
                        break;
                    case 5:
                        source.EnableEffectState(EffectStateType.Invisibility);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.Invisibility, 2);
                        break;
                }
            }
        }
    }
}
