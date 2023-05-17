namespace Common
{
    using Bowser.GameIntegration;
    using Bowser.Legacy;
    using HarmonyLib;

    internal static class Patcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateHobbyShop), "Start"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(GameStateHobbyShop_Start_Postfix)));
        }

        private static void GameStateHobbyShop_Start_Postfix(GameStateHobbyShop __instance)
        {
            var buttonHandler = Traverse.Create(__instance).Field<BowserButtonHandler>("buttonHandler").Value;
            CommonModule.HangoutsButtonHandler = buttonHandler;
        }
    }
}
