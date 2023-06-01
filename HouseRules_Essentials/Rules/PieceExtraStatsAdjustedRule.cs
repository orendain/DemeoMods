namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceExtraStatsAdjustedRule : Rule, IConfigWritable<Dictionary<BoardPieceId, int>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Piece max stat change effects added on creation";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private static Dictionary<BoardPieceId, int> _globalAdjustments;
        private static bool _isActivated;

        public PieceExtraStatsAdjustedRule(Dictionary<BoardPieceId, int> adjustments)
        {
            _adjustments = adjustments;
        }

        private readonly Dictionary<BoardPieceId, int> _adjustments;

        public Dictionary<BoardPieceId, int> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
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
            if (!_isActivated || !__result.IsPlayer())
            {
                return;
            }

            if (!_globalAdjustments.ContainsKey(__result.boardPieceId))
            {
                return;
            }

            foreach (var replacement in _globalAdjustments)
            {
                if (replacement.Key == __result.boardPieceId)
                {
                    __result.effectSink.TrySetStatMaxValue(Stats.Type.Strength, replacement.Value);
                    __result.effectSink.TrySetStatMaxValue(Stats.Type.Speed, replacement.Value);
                    __result.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, replacement.Value);
                    return;
                }
            }
        }
    }
}
