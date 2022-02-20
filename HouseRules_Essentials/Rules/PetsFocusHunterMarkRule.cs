namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Linq;
    using Boardgame;
    using Boardgame.Board;
    using Boardgame.Board.HeatMaps;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PetsFocusHunterMarkRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Pets focus on hunter marked enemies";

        private static bool _isActivated;

        public PetsFocusHunterMarkRule(bool enabled)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(BoardQuery), "UpdateCachedAttackTarget"),
                prefix: new HarmonyMethod(
                    typeof(PetsFocusHunterMarkRule),
                    nameof(BoardQuery_UpdateCachedAttackTarget_Prefix_Wrapper)));
        }

        private static void BoardQuery_UpdateCachedAttackTarget_Prefix_Wrapper(BoardQuery __instance)
        {
            try
            {
                BoardQuery_UpdateCachedAttackTarget_Prefix(__instance);
            }
            catch (Exception e)
            {
                EssentialsMod.Logger.Warning($"This should not have happened. Please submit this log to HouseRules developers: {e}");
            }
        }

        private static void BoardQuery_UpdateCachedAttackTarget_Prefix(BoardQuery __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            var attackTargetType = Traverse.Create(__instance)
                .Method("GetAttackTargetType", __instance.piece)
                .GetValue<PieceType>();
            if (attackTargetType != PieceType.Enemy)
            {
                return;
            }

            var enemyPieces = __instance.pieceAndTurnController.GetEnemyPieces();
            var markedEnemies = enemyPieces.FindAll(p => p.HasEffectState(EffectStateType.MarkOfAvalon));
            if (!markedEnemies.Any())
            {
                return;
            }

            var levelHeatMaps = Traverse.Create(__instance).Field<LevelHeatMaps>("levelHeatMaps").Value;
            var closeTargets = markedEnemies.OrderBy(p => FindDistanceBetween(__instance.piece, p, levelHeatMaps));
            var closestTarget = closeTargets.First();

            var isInMeleeRange = Traverse.Create(__instance)
                .Method("GetIsInMeleeRange", __instance.piece, closestTarget.gridPos.min.x, closestTarget.gridPos.min.y)
                .GetValue<bool>();
            if (!isInMeleeRange)
            {
                return;
            }

            var cachedAttackTarget = default(AttackTarget);
            cachedAttackTarget.piece = closestTarget;
            cachedAttackTarget.tile = closestTarget.gridPos.min;
            cachedAttackTarget.pieceVisible = __instance.IsVisible(closestTarget.gridPos);
            cachedAttackTarget.inMeleeAttackRange = isInMeleeRange;

            Traverse.Create(__instance).Field<AttackTarget>("cachedAttackTarget").Value = cachedAttackTarget;
            Traverse.Create(__instance).Field<bool>("hasCachedAttackTarget").Value = true;
        }

        private static int FindDistanceBetween(Piece piece, Piece otherPiece, LevelHeatMaps levelHeatMaps)
        {
            return levelHeatMaps
                .GetPieceMoveMap(piece, 30)
                .GetSteps(otherPiece.gridPos.min.x, otherPiece.gridPos.min.y);
        }
    }
}
