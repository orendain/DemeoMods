namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Common.States;
    using Common.UI;
    using HarmonyLib;
    using MelonLoader;
    using Photon.Realtime;
    using UnityEngine;

    internal class RoomListUI : MonoBehaviour
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
            _roomListPanel = RoomListPanel.NewInstance(_uiHelper);
            Initialize();

            RoomFinderMod.Logger.Msg("Initialization complete.");
        }

        private void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

            if (RoomFinderState.IsRefreshingRoomList && RoomFinderState.HasRoomListUpdated)
            {
                RoomFinderState.IsRefreshingRoomList = false;
                RoomFinderState.HasRoomListUpdated = false;
                PopulateRoomList();
            }
        }

        private void Initialize()
        {
            this.transform.SetParent(_uiHelper.DemeoResource.LobbyAnchor.transform, worldPositionStays: true);
            this.transform.position = new Vector3(25, 30, 0);
            this.transform.rotation = Quaternion.Euler(0, 40, 0);

            _background = new GameObject("RoomListUIBackground");
            _background.AddComponent<MeshFilter>().mesh = _uiHelper.DemeoResource.MenuBoxMesh;
            _background.AddComponent<MeshRenderer>().material = _uiHelper.DemeoResource.MenuBoxMaterial;

            _background.transform.SetParent(this.transform, worldPositionStays: false);
            _background.transform.localPosition = new Vector3(0, -3.6f, 0);
            _background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            _background.transform.localScale = new Vector3(2, 1, 2.5f);

            var menuTitle = _uiHelper.CreateMenuHeaderText("Public Rooms");
            menuTitle.transform.SetParent(this.transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 2.375f, 0);

            var refreshButton = _uiHelper.CreateButton(RefreshRoomList);
            refreshButton.transform.SetParent(this.transform, worldPositionStays: false);
            refreshButton.transform.localPosition = new Vector3(0, 0.3f, 0);

            var refreshText = _uiHelper.CreateButtonText("Refresh");
            refreshText.transform.SetParent(this.transform, worldPositionStays: false);
            refreshText.transform.localPosition = new Vector3(0, 0.3f, 0);

            // TODO(orendain): Fix so that ray interacts with entire object.
            this.gameObject.AddComponent<BoxCollider>();

            _isInitialized = true;
        }

        private static void RefreshRoomList()
        {
            RoomFinderState.IsRefreshingRoomList = true;
            Traverse.Create(GameContextState.LobbyMenuController)
                .Method("QuickPlay", LevelSequence.GameType.Invalid, true)
                .GetValue();
        }

        private void PopulateRoomList()
        {
            var cachedRooms =
                Traverse.Create(GameContextState.GameContext.gameStateMachine)
                    .Field<Dictionary<string, RoomInfo>>("cachedRoomList").Value;
            RoomFinderMod.Logger.Msg($"[RoomListUI] Retrieved {cachedRooms.Count} rooms.");

            var roomListPanelContainer = _roomListPanel.Reinitialize(cachedRooms.Values.ToList());
            roomListPanelContainer.transform.SetParent(this.transform, worldPositionStays: false);
            roomListPanelContainer.transform.localPosition = new Vector3(0, -1, 0);
        }
    }
}
