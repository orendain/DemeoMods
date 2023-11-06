namespace RoomFinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
    using HarmonyLib;
    using Photon.Realtime;
    using Room = RoomFinder.UI.Room;

    internal static class RoomManager
    {
        private static GameContext? _gameContext;
        private static LobbyMatchmakingController? _lobbyMatchmakingController;
        private static Lobby? _lobby;
        private static bool _isRefreshingRoomList;

        internal static Action<List<Room>>? RoomListUpdated { get; set; }

        /// <summary>
        /// Refreshes the list of rooms that the client knows about.
        /// </summary>
        internal static void RefreshRoomList()
        {
            if (_lobby == null)
            {
                RoomFinderBase.LogWarning("Lobby is uninitialized. Skipping room refresh.");
                return;
            }

            _isRefreshingRoomList = true;
            var lobbyMenuContext = Traverse
                .Create(_lobby.GetLobbyMenuController)
                .Field<LobbyMenu.ILobbyMenuContext>("lobbyMenuContext")
                .Value;

            lobbyMenuContext.QuickPlay(
                LevelSequence.GameType.Invalid,
                LevelSequence.ControlType.OneHero,
                matchMakeAnyGame: true,
                onError: null);
        }

        /// <summary>
        /// Joins the specified room.
        /// </summary>
        /// <param name="roomCode">The room code to join.</param>
        internal static void JoinRoom(string roomCode)
        {
            if (_lobby == null)
            {
                RoomFinderBase.LogWarning("Lobby is uninitialized. Skipping joining room.");
                return;
            }

            RoomFinderBase.LogDebug($"Joining room [{roomCode}].");
            Traverse.Create(_lobby.GetLobbyMenuController)
                .Method("JoinGame", roomCode, true)
                .GetValue();
        }

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(RoomManager), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(MatchmakingControllerFactory), "Create"),
                postfix: new HarmonyMethod(typeof(RoomManager), nameof(MatchmakingControllerFactory_Create_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(LobbyMatchmakingController), "OnRoomListUpdated"),
                postfix: new HarmonyMethod(
                    typeof(RoomManager),
                    nameof(LobbyMatchmakingController_OnRoomListUpdated_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(MatchMakingState), "OnMatchmakingRoomCodesUpdated"),
                prefix: new HarmonyMethod(
                    typeof(RoomManager),
                    nameof(MatchMakingState_OnMatchmakingRoomCodesUpdated_Prefix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            _gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _lobby = Traverse.Create(__instance).Field<Lobby>("lobby").Value;
        }

        private static void MatchmakingControllerFactory_Create_Postfix(MatchmakingController __result)
        {
            _lobbyMatchmakingController = __result.LobbyMatchmakingController;
        }

        private static void LobbyMatchmakingController_OnRoomListUpdated_Postfix()
        {
            var unfilteredRooms =
                Traverse.Create(_lobbyMatchmakingController)
                    .Field<List<RoomInfo>>("roomList").Value;

            RoomFinderBase.LogDebug($"Found {unfilteredRooms.Count} total rooms.");

            var isRoomValidMethod =
                Traverse.Create(_lobbyMatchmakingController)
                    .Method("IsRoomValidWithCurrentConfiguration", new[] { typeof(RoomInfo) });

            var filteredRooms =
                unfilteredRooms.Where(info => isRoomValidMethod.GetValue<bool>(info))
                    .Select(Room.Parse)
                    .ToList();

            RoomListUpdated?.Invoke(filteredRooms);
        }

        private static bool MatchMakingState_OnMatchmakingRoomCodesUpdated_Prefix()
        {
            if (!_isRefreshingRoomList)
            {
                return true;
            }

            _gameContext.gameStateMachine.goBackToMenuState = true;
            return false;
        }
    }
}
