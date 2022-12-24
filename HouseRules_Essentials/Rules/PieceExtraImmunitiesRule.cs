﻿namespace HouseRules.Essentials.Rules
{
    using Boardgame;
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

            if (target.piece.IsImmuneToDamage())
            {
                return true;
            }

            if (target.piece.boardPieceId == BoardPieceId.Verochka && damage.HasTag(DamageTag.Ice) && !attacker.piece.HasPieceType(PieceType.Boss))
            {
                target.piece.effectSink.SubtractHealth(0);
                return false;
            }

            if (!target.piece.IsPlayer())
            {
                return true;
            }

            EssentialsMod.Logger.Msg("Barbarian check");
            if (target.piece.boardPieceId == BoardPieceId.HeroBarbarian && !attacker.HasPieceType(PieceType.Boss) && (damage.HasTag(DamageTag.Acid) || damage.AbilityKey == AbilityKey.Petrify))
            {
                EssentialsMod.Logger.Msg("Barbarian 0 damage");
                target.piece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (target.piece.boardPieceId == BoardPieceId.HeroHunter && !attacker.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Ice))
            {
                target.piece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (target.piece.boardPieceId == BoardPieceId.HeroGuardian && !attacker.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Fire))
            {
                target.piece.effectSink.SubtractHealth(0);
                return false;
            }
            else if (target.piece.boardPieceId == BoardPieceId.HeroSorcerer && !attacker.HasPieceType(PieceType.Boss) && damage.HasTag(DamageTag.Electricity))
            {
                target.piece.effectSink.SubtractHealth(0);
                return false;
            }

            EssentialsMod.Logger.Msg("Return true");
            return true;
        }
    }
}
