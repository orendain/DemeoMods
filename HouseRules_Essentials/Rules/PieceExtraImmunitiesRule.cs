﻿namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.Data;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceExtraImmunitiesRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Add extra immunities to pieces.";

        private static bool _isActivated;

        public PieceExtraImmunitiesRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Damage), "DealDamage", parameters: new[] { typeof(Target), typeof(Damage), typeof(IntPoint2D), typeof(Target), typeof(PieceAndTurnController), typeof(BoardModel), typeof(OverkillController), typeof(bool) }),
                prefix: new HarmonyMethod(
                    typeof(PieceExtraImmunitiesRule),
                    nameof(Damage_DealDamage_Prefix)));
        }

        private static bool Damage_DealDamage_Prefix(Target target, Damage damage, Target attacker)
        {
            if (!_isActivated)
            {
                return true;
            }

            Piece targetPiece = target.piece;
            Piece attackerPiece = attacker.piece;
            if (targetPiece.IsImmuneToDamage())
            {
                return true;
            }

            if (attackerPiece == null)
            {
                return true;
            }

            if (attackerPiece.boardPieceId == BoardPieceId.HeroGuardian && damage.AbilityKey == AbilityKey.WhirlwindAttack)
            {
                BoardPieceId targetId = targetPiece.boardPieceId;
                string targetString = targetId.ToString();
                bool canBeHit = true;
                if (targetId == BoardPieceId.EnemyTurret || targetId == BoardPieceId.SporeFungus || targetString.Contains("SandPile") || targetPiece.HasPieceType(PieceType.ExplodingLamp))
                {
                    canBeHit = false;
                }

                if (targetPiece.IsPlayer() || targetPiece.IsBot() || (targetPiece.IsProp() && canBeHit))
                {
                    return false;
                }
            }

            if (targetPiece.boardPieceId == BoardPieceId.Verochka && damage.HasTag(DamageTag.Ice) && !attackerPiece.HasPieceType(PieceType.Boss))
            {
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (targetPiece.boardPieceId == BoardPieceId.WarlockMinion && !attackerPiece.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Undefined))
            {
                targetPiece.DisableEffectState(EffectStateType.CorruptedRage);
                targetPiece.effectSink.TrySetStatBaseValue(Stats.Type.CorruptionAP, 0);
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }

            if (!targetPiece.IsPlayer())
            {
                return true;
            }

            if (targetPiece.boardPieceId == BoardPieceId.HeroBarbarian && !attackerPiece.HasPieceType(PieceType.Boss) && (damage.HasTag(DamageTag.Acid) || damage.AbilityKey == AbilityKey.Petrify))
            {
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (targetPiece.boardPieceId == BoardPieceId.HeroWarlock && !attackerPiece.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Undefined))
            {
                targetPiece.DisableEffectState(EffectStateType.CorruptedRage);
                targetPiece.effectSink.TrySetStatBaseValue(Stats.Type.CorruptionAP, 0);
                targetPiece.effectSink.TryAddActionPoints(1);
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (targetPiece.boardPieceId == BoardPieceId.HeroHunter && !attackerPiece.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Ice))
            {
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (targetPiece.boardPieceId == BoardPieceId.HeroGuardian && !attackerPiece.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Fire))
            {
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (targetPiece.boardPieceId == BoardPieceId.HeroSorcerer && !attackerPiece.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Electricity))
            {
                targetPiece.effectSink.SubtractHealth(0);
                return false;
            }

            return true;
        }
    }
}
