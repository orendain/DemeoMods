namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Linq;
    using Boardgame;
    using Boardgame.Board;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PetsFocusHunterMarkRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Pets focus on Hunter's marked enemies";

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
                original: AccessTools.Method(typeof(BoardQuery), "FindAttackTarget"),
                prefix: new HarmonyMethod(
                    typeof(PetsFocusHunterMarkRule),
                    nameof(BoardQuery_FindAttackTarget_Prefix_Wrapper)));
        }

        private static bool BoardQuery_FindAttackTarget_Prefix_Wrapper(BoardQuery __instance, ref AttackTarget __result)
        {
            try
            {
                return BoardQuery_FindAttackTarget_Prefix(__instance, ref __result);
            }
            catch (Exception e)
            {
                EssentialsMod.Logger.Warning(
                    $"This should not have happened. Please submit this log to HouseRules developers: {e}");
                return true;
            }
        }

        private static bool BoardQuery_FindAttackTarget_Prefix(BoardQuery __instance, ref AttackTarget __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (!__instance.piece.HasPieceType(PieceType.Bot) && !__instance.piece.IsConfused())
            {
                return true;
            }

            var enemyPieces = __instance.pieceAndTurnController.GetTeamPieces(Boardgame.BoardEntities.Team.Two);
            var markedEnemies = enemyPieces.FindAll(p => p.HasEffectState(EffectStateType.MarkOfAvalon));
            if (!markedEnemies.Any())
            {
                return true;
            }

            var targetsInRange = markedEnemies
                .Where(p => __instance.GetIsInMeleeRange(__instance.piece, p.gridPos))
                .ToList();
            if (targetsInRange.Count == 0)
            {
                return true;
            }

            __result = new AttackTarget(__instance, targetsInRange.First(), __instance.piece);
            return false;
        }
    }
}
