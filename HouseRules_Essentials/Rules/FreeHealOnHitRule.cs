namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class FreeHealOnHitRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hit restores health.";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;
        private static int phase = 0;

        private readonly List<BoardPieceId> _adjustments;

        public FreeHealOnHitRule(List<BoardPieceId> adjustments)
        {
            _adjustments = adjustments;
        }

        public List<BoardPieceId> GetConfigObject() => _adjustments;

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
                    typeof(FreeHealOnHitRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Piece mainTarget, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            // Serpent Lord targetted
            if ((source.IsPlayer() || source.IsBot()) && mainTarget != null && mainTarget.boardPieceId == BoardPieceId.WizardBoss)
            {
                if (mainTarget.HasEffectState(EffectStateType.Invisible))
                {
                    mainTarget.EnableEffectState(EffectStateType.Invulnerable1);
                }
            }

            // Serpent Lord boss fight begins
            if (source.boardPieceId == BoardPieceId.WizardBoss)
            {
                source.effectSink.TryGetStat(Stats.Type.DamageResist, out var damageResist);
                if (damageResist < 1)
                {
                    source.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1);
                    source.DisableEffectState(EffectStateType.Invulnerable1);
                    source.DisableEffectState(EffectStateType.Berserk);
                    source.effectSink.Heal(100);
                }
            }

            // Elven Queen boss fight
            if (source.boardPieceId == BoardPieceId.ElvenQueen)
            {
                source.effectSink.TryGetStat(Stats.Type.DamageResist, out var damageResist);
                if (damageResist < 1)
                {
                    source.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1);
                    source.EnableEffectState(EffectStateType.Deflect);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Deflect, 1);
                }

                if (source.GetHealth() < 21)
                {
                    source.EnableEffectState(EffectStateType.MagicShield1);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.MagicShield1, 69);
                    source.EnableEffectState(EffectStateType.Courageous);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Courageous, 69);
                }
                else
                {
                    int nextPhase;
                    int low = 1;
                    int high = 6;
                    if (source.GetHealth() > 59)
                    {
                        nextPhase = Random.Range(1, 6);
                    }
                    else if (source.GetHealth() < 40)
                    {
                        low = 2;
                        high = 4;
                        nextPhase = Random.Range(2, 4);
                    }
                    else
                    {
                        low = 3;
                        nextPhase = Random.Range(3, 6);
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
                            source.EnableEffectState(EffectStateType.MagicShield1);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.MagicShield1, 1);
                            break;
                        case 3:
                            source.EnableEffectState(EffectStateType.Courageous);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.Courageous, 2);
                            break;
                        case 4:
                            source.EnableEffectState(EffectStateType.Recovery);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.Recovery, 2);
                            break;
                        case 5:
                            source.EnableEffectState(EffectStateType.FireImmunity);
                            source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 2);
                            break;
                    }
                }
            }

            if (source.boardPieceId == BoardPieceId.WarlockMinion)
            {
                float maxHealth = source.GetMaxHealth();
                source.effectSink.TryGetStat(Stats.Type.DamageResist, out var damageResist);
                if (damageResist < 1)
                {
                    source.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1);
                }

                maxHealth /= 2;
                if (source.GetHealth() < maxHealth)
                {
                    source.EnableEffectState(EffectStateType.Frenzy);
                    source.effectSink.SetStatusEffectDuration(EffectStateType.Frenzy, 1);
                }
                else
                {
                    source.DisableEffectState(EffectStateType.Frenzy);
                }
            }

            if (!source.IsPlayer())
            {
                return;
            }

            if (diceResult != Dice.Outcome.Hit)
            {
                return;
            }

            int chance = Random.Range(1, 101);
            if (_globalAdjustments.Contains(source.boardPieceId))
            {
                int chance2 = Random.Range(1, 101);
                if (source.boardPieceId == BoardPieceId.HeroRogue)
                {
                    if (chance > 98 && chance2 > 50)
                    {
                        source.effectSink.Heal(2);
                        source.AnimateWobble();
                    }
                    else if (chance2 > 50)
                    {
                        source.effectSink.Heal(1);
                        source.AnimateWobble();
                    }
                }
                else if (chance > 98)
                {
                    source.effectSink.Heal(2);
                    source.AnimateWobble();
                }
                else
                {
                    source.effectSink.Heal(1);
                }
            }
            else if (chance > 98)
            {
                source.effectSink.Heal(1);
                source.AnimateWobble();
            }
        }
    }
}
