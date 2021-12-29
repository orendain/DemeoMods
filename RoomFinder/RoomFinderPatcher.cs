namespace RoomFinder
{
    using System.Reflection;
    using Common.States;
    using HarmonyLib;

    internal static class RoomFinderPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo().GetDeclaredMethod("OnRoomListUpdated"),
                postfix: new HarmonyMethod(typeof(RoomFinderPatcher), nameof(MatchMakingState_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo()
                    .GetDeclaredMethod("FindGame"),
                prefix: new HarmonyMethod(typeof(RoomFinderPatcher), nameof(MatchMakingState_FindGame_Prefix)));
        }

        private static void MatchMakingState_OnRoomListUpdated_Postfix()
        {
            RoomFinderState.HasRoomListUpdated = true;
        }

        private static bool MatchMakingState_FindGame_Prefix()
        {
            if (RoomFinderState.IsRefreshingRoomList)
            {
                GameContextState.GameContext.gameStateMachine.goBackToMenuState = true;
                return false;
            }

            return true;
        }
    }
}
