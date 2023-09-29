namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
    using Common.UI;
    using Common.UI.Element;
    using HarmonyLib;
    using Photon.Realtime;
    using UnityEngine;

    internal class RoomFinderUiVr : MonoBehaviour
    {
        private VrResourceTable _resourceTable;
        private VrElementCreator _elementCreator;
        private RoomListPanelVr _roomListPanel;
        private Transform _anchor;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!VrElementCreator.IsReady()
                   || Resources
                       .FindObjectsOfTypeAll<charactersoundlistener>()
                       .Count(x => x.name == "MenuBox_BindPose") < 2)
            {
<<<<<<< HEAD
                RoomFinderCore.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderCore.LogDebug("UI dependencies ready. Proceeding with initialization.");
=======
                RoomFinderBase.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderBase.LogDebug("UI dependencies ready. Proceeding with initialization.");
>>>>>>> main

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = VrElementCreator.Instance();
            _roomListPanel = RoomListPanelVr.NewInstance(_elementCreator, RefreshRoomList);
            _anchor = Resources
                .FindObjectsOfTypeAll<charactersoundlistener>()
                .First(x => x.name == "MenuBox_BindPose").transform;

            Initialize();
<<<<<<< HEAD
            RoomFinderCore.LogDebug("Initialization complete.");
=======
            RoomFinderBase.LogDebug("Initialization complete.");
>>>>>>> main
        }

        private void Update()
        {
<<<<<<< HEAD
            if (!RoomFinderCore.SharedState.IsRefreshingRoomList)
=======
            if (!RoomFinderBase.SharedState.IsRefreshingRoomList)
>>>>>>> main
            {
                return;
            }

<<<<<<< HEAD
            if (!RoomFinderCore.SharedState.HasRoomListUpdated)
=======
            if (!RoomFinderBase.SharedState.HasRoomListUpdated)
>>>>>>> main
            {
                return;
            }

<<<<<<< HEAD
            RoomFinderCore.SharedState.IsRefreshingRoomList = false;
            RoomFinderCore.SharedState.HasRoomListUpdated = false;
=======
            RoomFinderBase.SharedState.IsRefreshingRoomList = false;
            RoomFinderBase.SharedState.HasRoomListUpdated = false;
>>>>>>> main
            PopulateRoomList();
        }

        private void Initialize()
        {
            transform.SetParent(_anchor, worldPositionStays: true);
            transform.position = new Vector3(25, 30, 0);
            transform.rotation = Quaternion.Euler(0, 40, 0);

            var background = new GameObject("RoomFinderUIBackground");
            background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            background.transform.SetParent(transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, -3.6f, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            background.transform.localScale = new Vector3(2, 1, 2.5f);

            var headerText = _elementCreator.CreateMenuHeaderText("RoomFinder");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            headerText.transform.localPosition = new Vector3(0, 2.375f, VrElementCreator.TextZShift);

            var selectionPanel = _roomListPanel.Panel;
            selectionPanel.transform.SetParent(transform, worldPositionStays: false);

<<<<<<< HEAD
            var versionText = _elementCreator.CreateNormalText($"v{RoomFinderCore.ModVersion}");
=======
            var versionText = _elementCreator.CreateNormalText($"v{RoomFinderBase.ModVersion}");
>>>>>>> main
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-3.25f, -19.5f, VrElementCreator.TextZShift);

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();
        }

        private static void RefreshRoomList()
        {
<<<<<<< HEAD
            RoomFinderCore.SharedState.IsRefreshingRoomList = true;
            var lobbyMenuController = Traverse
                .Create(RoomFinderCore.SharedState.GameContext.gameStateMachine.lobby)
=======
            RoomFinderBase.SharedState.IsRefreshingRoomList = true;
            var lobbyMenuController = Traverse
                .Create(RoomFinderBase.SharedState.GameContext.gameStateMachine.lobby)
>>>>>>> main
                .Field<LobbyMenuController>("lobbyMenuController")
                .Value;
            var lobbyMenuContext = Traverse
                .Create(lobbyMenuController)
                .Field<LobbyMenu.ILobbyMenuContext>("lobbyMenuContext")
                .Value;

            lobbyMenuContext.QuickPlay(
                LevelSequence.GameType.Invalid,
                LevelSequence.ControlType.OneHero,
                matchMakeAnyGame: true,
                onError: null);
        }

        private void PopulateRoomList()
        {
            var unfilteredRooms =
<<<<<<< HEAD
                Traverse.Create(RoomFinderCore.SharedState.LobbyMatchmakingController)
                    .Field<List<RoomInfo>>("roomList").Value;

            var isRoomValidMethod =
                Traverse.Create(RoomFinderCore.SharedState.LobbyMatchmakingController)
=======
                Traverse.Create(RoomFinderBase.SharedState.LobbyMatchmakingController)
                    .Field<List<RoomInfo>>("roomList").Value;

            var isRoomValidMethod =
                Traverse.Create(RoomFinderBase.SharedState.LobbyMatchmakingController)
>>>>>>> main
                    .Method("IsRoomValidWithCurrentConfiguration", new[] { typeof(RoomInfo) });

            var filteredRooms =
                unfilteredRooms.Where(info => isRoomValidMethod.GetValue<bool>(info))
                    .Select(Room.Parse)
                    .ToList();

<<<<<<< HEAD
            RoomFinderCore.LogInfo($"Found {unfilteredRooms.Count} total rooms.");
            RoomFinderCore.LogInfo($"Listing {filteredRooms.Count} available rooms.");
=======
            RoomFinderBase.LogInfo($"Found {unfilteredRooms.Count} total rooms.");
            RoomFinderBase.LogInfo($"Listing {filteredRooms.Count} available rooms.");
>>>>>>> main

            _roomListPanel.UpdateRooms(filteredRooms);
        }
    }
}
