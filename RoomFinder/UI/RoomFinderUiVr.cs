namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Common.UI;
    using Common.UI.Element;
    using HarmonyLib;
    using Photon.Realtime;
    using UnityEngine;

    internal class RoomFinderUiVr : MonoBehaviour
    {
        private bool _isInitialized;
        private VrResourceTable _resourceTable;
        private VrElementCreator _elementCreator;
        private GameObject _background;
        private RoomListPanel _roomListPanel;
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
            _roomListPanel = RoomListPanel.NewInstance(_elementCreator, RefreshRoomList);
            _anchor = Resources
                .FindObjectsOfTypeAll<charactersoundlistener>()
                .First(x => x.name == "MenuBox_BindPose").transform;

            Initialize();
            RoomFinderMod.Logger.Msg("Initialization complete.");
        }

        private void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

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

            _background = new GameObject("RoomFinderUIBackground");
            _background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            _background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            _background.transform.SetParent(transform, worldPositionStays: false);
            _background.transform.localPosition = new Vector3(0, -3.6f, 0);
            _background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            _background.transform.localScale = new Vector3(2, 1, 2.5f);

            var menuTitle = _elementCreator.CreateMenuHeaderText("RoomFinder");
            menuTitle.transform.SetParent(transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 2.375f, VrElementCreator.TextZShift);

            _roomListPanel.Panel.transform.SetParent(transform, worldPositionStays: false);

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-3.25f, -19.5f, VrElementCreator.TextZShift);

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();

            _isInitialized = true;
        }

        private static void RefreshRoomList()
        {
            RoomFinderMod.SharedState.IsRefreshingRoomList = true;
            Traverse.Create(RoomFinderMod.SharedState.GameContext.gameStateMachine.lobby.GetLobbyMenuController)
                .Method("QuickPlay", LevelSequence.GameType.Invalid, true)
                .GetValue();
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
