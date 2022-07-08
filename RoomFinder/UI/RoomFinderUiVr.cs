﻿namespace RoomFinder.UI
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
                RoomFinderMod.Logger.Msg("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderMod.Logger.Msg("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = VrElementCreator.Instance();
            _roomListPanel = RoomListPanelVr.NewInstance(_elementCreator, RefreshRoomList);
            _anchor = Resources
                .FindObjectsOfTypeAll<charactersoundlistener>()
                .First(x => x.name == "MenuBox_BindPose").transform;

            Initialize();
            RoomFinderMod.Logger.Msg("Initialization complete.");
        }

        private void Update()
        {
            if (!RoomFinderMod.SharedState.IsRefreshingRoomList)
            {
                return;
            }

            if (!RoomFinderMod.SharedState.HasRoomListUpdated)
            {
                return;
            }

            RoomFinderMod.SharedState.IsRefreshingRoomList = false;
            RoomFinderMod.SharedState.HasRoomListUpdated = false;
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

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-3.25f, -19.5f, VrElementCreator.TextZShift);

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();
        }

        private static void RefreshRoomList()
        {
            RoomFinderMod.SharedState.IsRefreshingRoomList = true;
            var lobbyMenuContext = Traverse
                .Create(RoomFinderMod.SharedState.GameContext.gameStateMachine.lobby.GetLobbyMenuController)
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
            var cachedRooms =
                Traverse.Create(RoomFinderMod.SharedState.GameContext.gameStateMachine)
                    .Field<Dictionary<string, RoomInfo>>("cachedRoomList").Value;

            RoomFinderMod.Logger.Msg($"Captured {cachedRooms.Count} rooms.");

            var rooms = cachedRooms.Values.ToList().Select(Room.Parse).ToList();
            _roomListPanel.UpdateRooms(rooms);
        }
    }
}
