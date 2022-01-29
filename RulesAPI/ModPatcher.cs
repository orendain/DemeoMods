namespace RulesAPI
{
    using Boardgame.BoardgameActions;
    using HarmonyLib;

    internal class ModPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(BoardGameActionStartNewGame), "StartNewGame"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(BoardGameActionStartNewGame_StartNewGame_Postfix)));

            // TODO(orendain): Hook into game ending events in order to deactivate activated rules.
        }

        private static void BoardGameActionStartNewGame_StartNewGame_Postfix()
        {
            // TODO(orendain): Reintroduce checks on private/public gamemode.
            RulesAPI.ActivateSelectedRuleset();
        }
    }
}
