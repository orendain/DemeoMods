namespace RoomFinder
{
    using System.Reflection;
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
    using Common;
    using HarmonyLib;

    internal static class Patcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools
                    .Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo()
                    .GetDeclaredMethod("OnRoomListUpdated"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(MatchMakingState_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools
                    .Inner(typeof(GameStateMachine), "MatchMakingState").GetTypeInfo()
                    .GetDeclaredMethod("FindGame"),
                prefix: new HarmonyMethod(typeof(Patcher), nameof(MatchMakingState_FindGame_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Lobby), "HideMenu"),
                prefix: new HarmonyMethod(typeof(Patcher), nameof(Lobby_HideMenu_Prefix)));
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

        private static bool Lobby_HideMenu_Prefix(Lobby __instance)
        {
            if (!CommonModule.IsPcEdition())
            {
                return true;
            }

            if (!RoomFinderMod.SharedState.IsRefreshingRoomList)
            {
                return true;
            }

            __instance.GetLobbyMenuController.view.ShowMainContent(LobbyMenuController.MenuContent.Play);
            return false;
        }
    }
}
