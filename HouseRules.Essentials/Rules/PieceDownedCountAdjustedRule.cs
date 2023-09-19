﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.LevelLoading;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class PieceDownedCountAdjustedRule : Rule, IConfigWritable<Dictionary<BoardPieceId, int>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero maximum knockdown count adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private static Dictionary<BoardPieceId, int> _globalAdjustments;
        private static bool _isActivated;

        public PieceDownedCountAdjustedRule(Dictionary<BoardPieceId, int> adjustments)
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
                    typeof(PieceDownedCountAdjustedRule),
                    nameof(CreatePiece_RecreatePieceOnNewLevel_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(LevelLoaderAndInitializer), "RecreatePieceOnNewLevel"),
                postfix: new HarmonyMethod(
                    typeof(PieceDownedCountAdjustedRule),
                    nameof(CreatePiece_RecreatePieceOnNewLevel_Postfix)));
        }

        private static void CreatePiece_RecreatePieceOnNewLevel_Postfix(ref Piece __result)
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
                int countReplacement;
                switch (replacement.Value)
                {
                    case 0:
                        countReplacement = 3;
                        break;
                    case 1:
                        countReplacement = 2;
                        break;
                    case 2:
                        countReplacement = 1;
                        break;
                    default:
                        countReplacement = 0;
                        break;
                }

                if (replacement.Key == __result.boardPieceId)
                {
                    __result.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, countReplacement);
                }
            }
        }
    }
}
