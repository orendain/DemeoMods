namespace RoomFinder
{
    using Boardgame;
    using Boardgame.Networking;
    using Boardgame.PlayerData;
    using ExternalMatchmaking;
    using HarmonyLib;

    internal static class Patcher
    {
        internal static void Patch(HarmonyLib.Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(MatchmakingController),
                    new[]
                    {
                        typeof(SerializableEventQueue),
                        typeof(INetworkController),
                        typeof(ConnectionStringProvider),
                        typeof(PlayerDataController),
                        typeof(UserBlockingController),
                        typeof(ReconnectController),
                        typeof(VoiceSettings),
                        typeof(AvatarController),
                        typeof(IExternalMatchmaking),
                    }),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(MatchmakingController_Constructor_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(LobbyMatchmakingController), "OnRoomListUpdated"),
                postfix: new HarmonyMethod(typeof(Patcher), nameof(LobbyMatchmakingController_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(MatchMakingState), "OnMatchmakingRoomCodesUpdated"),
                prefix: new HarmonyMethod(typeof(Patcher), nameof(MatchMakingState_OnMatchmakingRoomCodesUpdated_Prefix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            RoomFinderBase.SharedState.GameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
        }

        private static void MatchmakingController_Constructor_Postfix(MatchmakingController __instance)
        {
            RoomFinderBase.SharedState.LobbyMatchmakingController = __instance.LobbyMatchmakingController;
        }

        private static void LobbyMatchmakingController_OnRoomListUpdated_Postfix()
        {
            RoomFinderBase.SharedState.HasRoomListUpdated = true;
        }

        private static bool MatchMakingState_OnMatchmakingRoomCodesUpdated_Prefix()
        {
            if (!RoomFinderBase.SharedState.IsRefreshingRoomList)
            {
                return true;
            }

            RoomFinderBase.SharedState.GameContext.gameStateMachine.goBackToMenuState = true;
            return false;
        }
    }
}
