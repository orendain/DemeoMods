﻿namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common.UI;
    using Common.UI.Element;
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
            RoomManager.RoomListUpdated += OnRoomListUpdated;
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!VrElementCreator.IsReady()
                   || Resources
                       .FindObjectsOfTypeAll<charactersoundlistener>()
                       .Count(x => x.name == "MenuBox_BindPose") < 2)
            {
                RoomFinderBase.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderBase.LogDebug("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = VrElementCreator.Instance();
            _roomListPanel = RoomListPanelVr.NewInstance(_elementCreator, RoomManager.RefreshRoomList);
            _anchor = Resources
                .FindObjectsOfTypeAll<charactersoundlistener>()
                .First(x => x.name == "MenuBox_BindPose").transform;

            Initialize();
            RoomFinderBase.LogDebug("Initialization complete.");
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

            var versionText = _elementCreator.CreateNormalText($"v{RoomFinderBase.ModVersion}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-3.25f, -19.5f, VrElementCreator.TextZShift);

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();
        }

        private void OnRoomListUpdated(List<Room> rooms)
        {
            RoomFinderBase.LogInfo($"Listing {rooms.Count} available rooms.");
            _roomListPanel.UpdateRooms(rooms);
        }
    }
}
