namespace RulesAPI
{
    using System.Reflection;
    using Boardgame.Networking;
    using HarmonyLib;

    internal class ModPatcher
    {
        private static bool _isCreatingGame;

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo().GetDeclaredMethod("Enter"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_Enter_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToPlayingState"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_GoToPlayingState_Postfix)));

            // TODO(orendain): Hook into game ending events in order to deactivate activated rules.
        }

        private static void GameStateMachine_Enter_Prefix(GameStateMachine gsm)
        {
            var createGameMode = Traverse.Create(gsm).Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            _isCreatingGame = true;
        }

        private static void GameStateMachine_GoToPlayingState_Postfix()
        {
            // TODO(orendain): Finding more appropriate hook locations than GSM's GoToPlayingState,
            // which is called at the end of BoardGameActionStartNewGame.
            if (_isCreatingGame)
            {
                _isCreatingGame = false;
                RulesAPI.ActivateSelectedRuleset();
            }
        }
    }
}
