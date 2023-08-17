namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceKeyholderRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Holding the key gives certain advantages...";

        private static bool _isActivated;
        private static int _keyResist;

        public PieceKeyholderRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
            _keyResist = 0;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(PieceKeyholderRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static void Inventory_RestoreReplenishables_Prefix(Piece piece)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            // Handle keyholder gets 1 damage resist and 1 counter-attack damage or points if using PointGainRule
            if (piece.HasEffectState(EffectStateType.Locked))
            {
                if (!piece.HasEffectState(EffectStateType.Key))
                {
                    if (!piece.HasEffectState(EffectStateType.StrengthInNumbers))
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, _keyResist);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.DamageResist, 1);
                        if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                        }
                        else
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 0);
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                        }

                        piece.DisableEffectState(EffectStateType.Locked);
                    }
                    else
                    {
                        piece.DisableEffectState(EffectStateType.Locked);
                    }
                }
                else if (piece.HasEffectState(EffectStateType.StrengthInNumbers))
                {
                    var pointCount = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Locked);
                    if (pointCount > 1)
                    {
                        pointCount--;
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Locked);
                        piece.effectSink.AddStatusEffect(EffectStateType.Locked, pointCount);
                    }
                }
            }
            else if (piece.HasEffectState(EffectStateType.Key))
            {
                if (!piece.HasEffectState(EffectStateType.Locked))
                {
                    if (!piece.HasEffectState(EffectStateType.StrengthInNumbers))
                    {
                        _keyResist = 0;
                        piece.effectSink.AddStatusEffect(EffectStateType.Locked, -1);
                        if (piece.GetStat(Stats.Type.DamageResist) > 0)
                        {
                            _keyResist = 1;
                        }

                        piece.effectSink.TrySetStatMaxValue(Stats.Type.DamageResist, 2);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1 + _keyResist);
                        if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 2);
                        }
                        else
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 255);
                        }
                    }
                    else
                    {
                        piece.effectSink.AddStatusEffect(EffectStateType.Locked, PointGainRule._globalConfig.UnlockDoor);
                    }
                }
            }
        }
    }
}
