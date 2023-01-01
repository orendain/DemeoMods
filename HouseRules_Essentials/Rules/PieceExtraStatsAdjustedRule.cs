namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceExtraStatsAdjustedRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Piece effects added on creation";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private static bool _isActivated;

        public PieceExtraStatsAdjustedRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(PieceExtraStatsAdjustedRule),
                    nameof(CreatePiece_Effects_Postfix)));
        }

        private static void CreatePiece_Effects_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (__result.HasPieceType(PieceType.Boss) || __result.boardPieceId == BoardPieceId.WarlockMinion)
            {
                __result.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1);
            }
            else if (__result.IsPlayer())
            {
                __result.effectSink.TrySetStatMaxValue(Stats.Type.Strength, 5);
                __result.effectSink.TrySetStatMaxValue(Stats.Type.Speed, 5);
                __result.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, 5);

                if (__result.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                    __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 255);
                }
            }
        }
    }
}
