namespace Common
{
    using Bowser.Core;
    using Bowser.GameIntegration;
    using HarmonyLib;

    internal static class ModPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateHobbyShop), "Start"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(GameStateHobbyShop_Start_Postfix)));
        }

        private static void GameStateHobbyShop_Start_Postfix(GameStateHobbyShop __instance)
        {
            var buttonHandler = Traverse.Create(__instance).Field<BowserButtonHandler>("buttonHandler").Value;
            CommonModule.HangoutsButtonHandler = buttonHandler;
        }
    }
}
