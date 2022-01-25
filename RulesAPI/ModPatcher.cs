namespace RulesAPI
{
    using System.Reflection;
    using Boardgame.Networking;
    using HarmonyLib;

    internal class ModPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo().GetDeclaredMethod("Enter"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateMachine_Enter_Prefix)));

            // TODO(orendain): Hook into game ending events in order to deactivate activated rules.
        }

        private static void GameStateMachine_Enter_Prefix(GameStateMachine gsm)
        {
            var createGameMode = Traverse.Create(gsm).Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            // TODO(orendain): Ensure rules are also activated for offline skirmishes.
            RulesAPIMod.ActivateSelectedRuleset();
        }
    }
}
