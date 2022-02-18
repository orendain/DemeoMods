namespace HouseRules.Essentials.Rules
{
    using System.Collections;
    using Boardgame;
    using Boardgame.BoardgameActions;
    using Boardgame.LevelLoading;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class LevelExitLockedUntilAllEnemiesDeadRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Level exit stays locked until all enemies are dead.";

        private static bool _isActivated;

        public LevelExitLockedUntilAllEnemiesDeadRule(bool enabled)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(LevelManager), "LoadAndSetupLevel"),
                postfix: new HarmonyMethod(
                    typeof(LevelExitLockedUntilAllEnemiesDeadRule),
                    nameof(LevelManager_LoadAndSetupLevel_Postfix)));

            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(BoardgameActionPieceDied),
                    new[] { typeof(GameContext), typeof(int[]), typeof(int) }),
                postfix: new HarmonyMethod(
                    typeof(LevelExitLockedUntilAllEnemiesDeadRule),
                    nameof(BoardgameActionPieceDied_Constructor_Postfix)));
        }

        private static void LevelManager_LoadAndSetupLevel_Postfix(LevelManager __instance, ref IEnumerator __result)
        {
            if (!_isActivated)
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
            var keyHolder = gameContext.pieceAndTurnController.FindFirstPiece(p => p.HasEffectState(EffectStateType.Key));
            if (keyHolder == null)
            {
                EssentialsMod.Logger.Warning("[LevelExitLockedUntilAllEnemiesDeadRule] Could not find key holder on this level. Skipping.");
                return;
            }

            keyHolder.DisableEffectState(EffectStateType.Key);
        }

        private static void BoardgameActionPieceDied_Constructor_Postfix(GameContext context)
        {
            if (!_isActivated)
            {
                return;
            }

            if (context.pieceAndTurnController.GetEnemyPieces().Count != 0)
            {
                return;
            }

            context.pieceAndTurnController.FindFirstPiece(p => p.HasPieceType(PieceType.LevelExit))?.DisableEffectState(EffectStateType.Locked);
            GameUI.ShowCameraMessage("Last enemy has been killed. Exit unlocked.", 5);
        }
    }
}
