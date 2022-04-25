namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class BackstabConfigOverriddenRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Backstab is allocated to new BoardPieceIDs";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public BackstabConfigOverriddenRule(List<BoardPieceId> adjustments)
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
                original: AccessTools.Method(typeof(Piece), "IsRogue"),
                prefix: new HarmonyMethod(
                    typeof(BackstabConfigOverriddenRule),
                    nameof(Piece_IsRogue_Prefix)));
        }

        private static bool Piece_IsRogue_Prefix(Piece __instance, ref bool __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = _globalAdjustments.Contains(__instance.boardPieceId);
            return false; // We returned an user-adjusted config.
        }
    }
}
