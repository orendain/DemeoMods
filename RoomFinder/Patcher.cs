namespace RoomFinder
{
    using System.Reflection;
    using Boardgame;
    using HarmonyLib;
    using static UnityEngine.UI.Image;

    internal static class Patcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(GameStartup_InitializeGame_Postfix)));
            harmony.Patch(
                original: AccessTools.Method(typeof(MatchMakingState), "OnRoomListUpdated"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(MatchMakingState_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(MatchMakingState), "FindGame"),
                prefix: new HarmonyMethod(typeof(Patcher), nameof(MatchMakingState_FindGame_Prefix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            RoomFinderMod.SharedState.GameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
        }

        private static void MatchMakingState_OnRoomListUpdated_Postfix()
        {
            RoomFinderMod.SharedState.HasRoomListUpdated = true;
        }

        private static bool MatchMakingState_FindGame_Prefix()
        {
            if (!RoomFinderMod.SharedState.IsRefreshingRoomList)
            {
                return true;
            }

            RoomFinderMod.SharedState.GameContext.gameStateMachine.goBackToMenuState = true;
            return false;
        }
    }
}
