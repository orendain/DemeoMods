using System.Reflection;
using Common.States;
using HarmonyLib;

namespace RoomFinder
{
    internal static class RoomFinderPatcher
    {
        internal static void Patch(HarmonyLib.Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo().GetDeclaredMethod("OnRoomListUpdated"),
                postfix: new HarmonyMethod(typeof(RoomFinderPatcher), nameof(MatchMakingState_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools.Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo()
                    .GetDeclaredMethod("FindGame"),
                prefix: new HarmonyMethod(typeof(RoomFinderPatcher),
                    nameof(MatchMakingState_FindGame_Prefix)));
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
