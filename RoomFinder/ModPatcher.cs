namespace RoomFinder
{
    using System.Reflection;
    using HarmonyLib;

    internal static class ModPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo()
                    .GetDeclaredMethod("OnRoomListUpdated"),
                postfix: new HarmonyMethod(typeof(ModPatcher), nameof(MatchMakingState_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo()
                    .GetDeclaredMethod("FindGame"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(MatchMakingState_FindGame_Prefix)));
        }

        private static void MatchMakingState_OnRoomListUpdated_Postfix()
        {
            RoomFinderMod.ModState.HasRoomListUpdated = true;
        }

        private static bool MatchMakingState_FindGame_Prefix()
        {
            if (RoomFinderMod.ModState.IsRefreshingRoomList)
            {
                RoomFinderMod.GameContextState.GameContext.gameStateMachine.goBackToMenuState = true;
                return false;
            }

            return true;
        }
    }
}
