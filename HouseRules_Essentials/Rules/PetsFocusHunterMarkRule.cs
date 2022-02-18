namespace HouseRules.Essentials.Rules
{
    using System.Linq;
    using Boardgame;
    using Boardgame.Board;
    using Boardgame.Board.HeatMaps;
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
                    nameof(BoardQuery_UpdateCachedAttackTarget_Prefix)));
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
            var closestTarget = markedEnemies.OrderByDescending(p =>
                levelHeatMaps.GetPieceMoveMap(__instance.piece, 30).GetSteps(p.gridPos.min.x, p.gridPos.min.y)).First();

            var cachedAttackTarget = default(AttackTarget);
            cachedAttackTarget.piece = closestTarget;
            cachedAttackTarget.tile = closestTarget.gridPos.min;
            cachedAttackTarget.pieceVisible = __instance.IsVisible(closestTarget.gridPos);
            cachedAttackTarget.inMeleeAttackRange = Traverse.Create(__instance)
                .Method("GetIsInMeleeRange", __instance.piece, closestTarget.gridPos.min.x, closestTarget.gridPos.min.y)
                .GetValue<bool>();

            Traverse.Create(__instance).Field<AttackTarget>("cachedAttackTarget").Value = cachedAttackTarget;
            Traverse.Create(__instance).Field<bool>("hasCachedAttackTarget").Value = true;
        }
    }
}
