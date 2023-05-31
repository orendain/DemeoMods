namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceDamageResistRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Piece damage resist effects added on creation";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public PieceDamageResistRule(List<BoardPieceId> adjustments)
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
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(PieceDamageResistRule),
                    nameof(CreatePiece_Effects_Postfix)));
        }

        private static void CreatePiece_Effects_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!_globalAdjustments.Contains(__result.boardPieceId))
            {
                return;
            }

            __result.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1);
            MelonLoader.MelonLogger.Msg($"{__result.boardPieceId} damage resist set to 1");
        }
    }
}
