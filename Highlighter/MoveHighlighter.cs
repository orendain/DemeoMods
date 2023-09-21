namespace Highlighter
{
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.BoardPiece;
    using Boardgame.Data;
    using Boardgame.NonVR.BoardPiece;
    using Boardgame.Ui;
    using DataKeys;
    using HarmonyLib;
    using UnityEngine;

    public static class MoveHighlighter
    {
        private static GameContext _gameContext;
        private static IntPoint2D _lastHoveredTile;

        internal static void Patch(Harmony harmony)
        {
            // To allow mid-game hooking.
            _gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(MoveHighlighter), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(NonVrGrabbableMiniature), "OnGrabMoved"),
                postfix: new HarmonyMethod(
                    typeof(MoveHighlighter),
                    nameof(NonVrGrabbableMiniature_OnGrabMoved_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GrabbableMiniature), "OnGrabMoved"),
                postfix: new HarmonyMethod(
                    typeof(MoveHighlighter),
                    nameof(GrabbableMiniature_OnGrabMoved_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(BaseGrabbableMiniature), "OnGrabStatusChanged"),
                postfix: new HarmonyMethod(
                    typeof(MoveHighlighter),
                    nameof(BaseGrabbableMiniature_OnGrabStatusChanged_Postfix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            _gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
        }

        private static void NonVrGrabbableMiniature_OnGrabMoved_Postfix(
            NonVrGrabbableMiniature __instance,
            Vector3 position)
        {
            var actionSelect = Traverse.Create(__instance).Field<ActionSelect>("actionSelect").Value;
            OnMiniatureGrabMoved(actionSelect, position);
        }

        private static void GrabbableMiniature_OnGrabMoved_Postfix(
            GrabbableMiniature __instance,
            Vector3 pos)
        {
            var actionSelect = Traverse.Create(__instance).Field<ActionSelect>("actionSelect").Value;
            OnMiniatureGrabMoved(actionSelect, pos);
        }

        private static void BaseGrabbableMiniature_OnGrabStatusChanged_Postfix(bool status)
        {
            // Do not react when piece is first picked up.
            if (status)
            {
                return;
            }

            ClearHighlightedLineOfSight();
        }

        private static void OnMiniatureGrabMoved(ActionSelect actionSelect, Vector3 position)
        {
            if (actionSelect == null)
            {
                return;
            }

            var hoveredTile = _gameContext.boardModel.GetTileHoverData(position).tile;
            var isMoveInvalid = !actionSelect.MovesInRange.IsMove(hoveredTile);
            if (isMoveInvalid)
            {
                // No redraw required.
                if (_lastHoveredTile == IntPoint2D.Invalid)
                {
                    return;
                }

                _lastHoveredTile = IntPoint2D.Invalid;
                ClearHighlightedLineOfSight();
                return;
            }

            // No redraw required.
            if (_lastHoveredTile == hoveredTile)
            {
                return;
            }

            _lastHoveredTile = hoveredTile;
            HighlightLineOfSight(hoveredTile);
        }

        private static void ClearHighlightedLineOfSight()
        {
            TileHighlightController.TileHighLight?.Clear();
        }

        private static void HighlightLineOfSight(IntPoint2D hoveredTile)
        {
            var currentPiece = _gameContext.pieceAndTurnController.CurrentPiece;
            var originalGridPos = currentPiece.gridPos;

            currentPiece.gridPos = originalGridPos.GetMoveTo(hoveredTile);
            var moveSet = AbilityFactory
                .GetAbility(AbilityKey.MagicMissile)
                .CreateMoveSet(currentPiece, _gameContext.pieceAndTurnController, _gameContext.boardModel);
            currentPiece.gridPos = originalGridPos;

            TileHighlightController.TileHighLight?.UpdateHighlights(
                moveSet,
                TileHighlight.HighlightType.ElvenKingShockwave,
                null,
                _gameContext.boardModel);
            TileHighlightController.TileHighLight?.FadeIn();
        }
    }
}
