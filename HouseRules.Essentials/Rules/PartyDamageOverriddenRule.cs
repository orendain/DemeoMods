namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.Data;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class PartyDamageOverriddenRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Electrical attacks won't affect other players, pets, or player made constructs";

        private static bool _isActivated;
        private static Piece? _targetPiece;

        public PartyDamageOverriddenRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(Context context) => _isActivated = true;

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Damage), "DealDamage", parameters: new[] { typeof(Target), typeof(Damage), typeof(IntPoint2D), typeof(Target), typeof(PieceAndTurnController), typeof(BoardModel), typeof(OverkillController), typeof(bool), typeof(bool), typeof(bool) }),
                prefix: new HarmonyMethod(
                    typeof(PartyDamageOverriddenRule),
                    nameof(Damage_DealDamage_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "EnableEffectState"),
                postfix: new HarmonyMethod(
                    typeof(PartyDamageOverriddenRule),
                    nameof(Piece_EnableEffectState_Postfix)));
        }

        private static void Piece_EnableEffectState_Postfix()
        {
            if (!_isActivated)
            {
                return;
            }

            if (_targetPiece != null)
            {
                if (!_targetPiece.IsImmuneToStatusEffect(EffectStateType.Stunned))
                {
                    _targetPiece.DisableEffectState(EffectStateType.Stunned);
                    _targetPiece.effectSink.SubtractHealth(0);
                }
            }

            _targetPiece = null;
        }

        private static bool Damage_DealDamage_Prefix(Target target, Damage damage, Target attacker)
        {
            if (!_isActivated)
            {
                return true;
            }

            Piece targetPiece = target.piece;
            if (targetPiece.IsImmuneToDamage())
            {
                return true;
            }

            Piece attackerPiece = attacker.piece;
            if (attackerPiece != null)
            {
                if (attackerPiece.IsPlayer() && (targetPiece.IsPlayer() || targetPiece.IsBot() || targetPiece.HasEffectState(EffectStateType.ConfusedPermanentVisualOnly)) && damage.HasTag(DamageTag.Electricity))
                {
                    targetPiece.effectSink.SubtractHealth(0);
                    if (!targetPiece.HasEffectState(EffectStateType.Stunned))
                    {
                        _targetPiece = targetPiece;
                    }

                    return false;
                }

                return true;
            }

            return true;
        }
    }
}
