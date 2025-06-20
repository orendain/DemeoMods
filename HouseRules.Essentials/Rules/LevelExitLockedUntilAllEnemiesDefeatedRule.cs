﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardgameActions;
    using Boardgame.LevelLoading;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class LevelExitLockedUntilAllEnemiesDefeatedRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Level exit stays locked until all enemies are defeated";

        private static bool _isActivated;

        public LevelExitLockedUntilAllEnemiesDefeatedRule(bool enabled)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(Context context) => _isActivated = true;

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(LevelLoaderAndInitializer), "LoadAndSetupLevel"),
                postfix: new HarmonyMethod(
                    typeof(LevelExitLockedUntilAllEnemiesDefeatedRule),
                    nameof(LevelLoaderAndInitializer_LoadAndSetupLevel_Postfix)));

            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(BoardgameActionPieceDied),
                    new[]
                    {
                        typeof(GameContext), typeof(int[]), typeof(bool), typeof(bool), typeof(bool), typeof(int),
                    }),
                postfix: new HarmonyMethod(
                    typeof(LevelExitLockedUntilAllEnemiesDefeatedRule),
                    nameof(BoardgameActionPieceDied_Constructor_Postfix)));
        }

        private static void LevelLoaderAndInitializer_LoadAndSetupLevel_Postfix(LevelLoaderAndInitializer __instance, ref IEnumerator __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (__instance.IsBossLevel())
            {
                return;
            }

            var gameContext = Traverse.Create(__instance).Property<GameContext>("gameContext").Value;
            __result = OverrideEnumerator(__result, gameContext);
        }

        private static IEnumerator OverrideEnumerator(IEnumerator enumerator, GameContext gameContext)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            RemoveKeyFromEnemies(gameContext);
        }

        private static void RemoveKeyFromEnemies(GameContext gameContext)
        {
            var keyHolder =
                gameContext.pieceAndTurnController.FindFirstPiece(p => p.HasEffectState(EffectStateType.Key));
            if (keyHolder == null)
            {
                HouseRulesEssentialsBase.LogWarning(
                    "[LevelExitLockedUntilAllEnemiesDefeated] Could not find key holder on this level. Skipping.");
                return;
            }

            keyHolder.DisableEffectState(EffectStateType.Key);
        }

        private static void BoardgameActionPieceDied_Constructor_Postfix(GameContext gameContext)
        {
            if (!_isActivated)
            {
                return;
            }

            if (IsEnemyRemaining(gameContext))
            {
                return;
            }

            if (gameContext.levelLoaderAndInitializer.IsBossLevel())
            {
                return;
            }

            GameUI.ShowCameraMessage("All enemies have been defeated! You may advance.", 5);

            var levelExit = gameContext.pieceAndTurnController.FindFirstPiece(p => p.HasPieceType(PieceType.LevelExit));
            levelExit?.DisableEffectState(EffectStateType.Locked);
            HR.ScheduleBoardSync();
        }

        private static bool IsEnemyRemaining(GameContext gameContext)
        {
            return gameContext.pieceAndTurnController.GetTeamPieces(Boardgame.BoardEntities.Team.Two).Where(p =>
            {
                if (p.boardPieceId == BoardPieceId.SpiderEgg)
                {
                    return false;
                }

                return !p.IsConfused();
            }).Any();
        }
    }
}
