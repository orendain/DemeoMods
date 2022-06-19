namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Common.UI;
    using HarmonyLib;
    using Photon.Realtime;
    using UnityEngine;

    internal class RoomFinderUI : MonoBehaviour
    {
        private bool _isInitialized;
        private UiHelper _uiHelper;
        private GameObject _background;
        private RoomListPanel _roomListPanel;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!UiHelper.IsReady())
            {
                RoomFinderMod.Logger.Msg("UI helper not yet ready. Trying again...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderMod.Logger.Msg("UI helper ready. Proceeding with initialization.");

            _uiHelper = UiHelper.Instance();
            _roomListPanel = RoomListPanel.NewInstance(_uiHelper, RefreshRoomList);
            Initialize();

            RoomFinderMod.Logger.Msg("Initialization complete.");
        }

        private void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

            if (!RoomFinderMod.ModState.IsRefreshingRoomList)
            {
                return;
            }

            if (!RoomFinderMod.ModState.HasRoomListUpdated)
            {
                return;
            }

            RoomFinderMod.ModState.IsRefreshingRoomList = false;
            RoomFinderMod.ModState.HasRoomListUpdated = false;
            PopulateRoomList();
        }

        private void Initialize()
        {
            transform.SetParent(_uiHelper.DemeoResource.VrLobbyTableAnchor.transform, worldPositionStays: true);
            transform.position = new Vector3(25, 30, 0);
            transform.rotation = Quaternion.Euler(0, 40, 0);

            _background = new GameObject("RoomFinderUIBackground");
            _background.AddComponent<MeshFilter>().mesh = _uiHelper.DemeoResource.MenuBoxMesh;
            _background.AddComponent<MeshRenderer>().material = _uiHelper.DemeoResource.MenuBoxMaterial;

            _background.transform.SetParent(transform, worldPositionStays: false);
            _background.transform.localPosition = new Vector3(0, -3.6f, 0);
            _background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            _background.transform.localScale = new Vector3(2, 1, 2.5f);

            var menuTitle = _uiHelper.CreateMenuHeaderText("RoomFinder");
            menuTitle.transform.SetParent(transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 2.375f, UiHelper.DefaultTextZShift);

            _roomListPanel.Panel.transform.SetParent(transform, worldPositionStays: false);

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();

            _isInitialized = true;
        }

        private static void RefreshRoomList()
        {
            RoomFinderMod.ModState.IsRefreshingRoomList = true;
            Traverse.Create(RoomFinderMod.ModState.GameContext.gameStateMachine.lobby.GetLobbyMenuController)
                .Method("QuickPlay", LevelSequence.GameType.Invalid, true)
                .GetValue();
        }

        private void PopulateRoomList()
        {
            var cachedRooms =
                Traverse.Create(RoomFinderMod.ModState.GameContext.gameStateMachine)
                    .Field<Dictionary<string, RoomInfo>>("cachedRoomList").Value;

            RoomFinderMod.Logger.Msg($"Captured {cachedRooms.Count} rooms.");

            var rooms = cachedRooms.Values.ToList().Select(Room.Parse).ToList();
            _roomListPanel.UpdateRooms(rooms);
        }
    }
}
