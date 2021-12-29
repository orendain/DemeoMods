﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boardgame;
using Common.States;
using Common.Ui;
using HarmonyLib;
using MelonLoader;
using Photon.Realtime;
using UnityEngine;

namespace RoomFinder.UI
{
    internal class RoomListUI : MonoBehaviour
    {
        private bool isInitialized;
        private UiUtil uiUtil;
        private GameObject background;
        private RoomListPanel roomListPanel;

        private void Start()
        {
            StartCoroutine(Setup());
        }

        private IEnumerator Setup()
        {
            while (!UiUtil.IsReady())
            {
                MelonLogger.Msg("UI utility not yet ready. Trying again...");
                yield return new WaitForSecondsRealtime(1);
            }

            MelonLogger.Msg("UI utility ready. Proceeding with setup.");

            uiUtil = UiUtil.Instance();
            roomListPanel = RoomListPanel.NewInstance(uiUtil);
            Initialize();
        }

        private void Update()
        {
            if (!isInitialized)
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
            this.transform.SetParent(uiUtil.DemeoUi.DemeoLobbyAnchor.transform, worldPositionStays: true);
            this.transform.position = new Vector3(25, 30, 0);
            this.transform.rotation = Quaternion.Euler(0, 40, 0);

            background = new GameObject("RoomListUIBackground");
            background.AddComponent<MeshFilter>().mesh = uiUtil.DemeoUi.DemeoMenuBoxMesh;
            background.AddComponent<MeshRenderer>().material = uiUtil.DemeoUi.DemeoMenuBoxMaterial;

            background.transform.SetParent(this.transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, -3.6f, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            background.transform.localScale = new Vector3(2, 1, 2.5f);

            var menuTitle = uiUtil.CreateMenuHeaderText("Public Rooms");
            menuTitle.transform.SetParent(this.transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 2.375f, 0);

            var refreshButton = uiUtil.CreateButton(RefreshRoomList);
            refreshButton.transform.SetParent(this.transform, worldPositionStays: false);
            refreshButton.transform.localPosition = new Vector3(0, 0.3f, 0);

            var refreshText = uiUtil.CreateButtonText("Refresh");
            refreshText.transform.SetParent(this.transform, worldPositionStays: false);
            refreshText.transform.localPosition = new Vector3(0, 0.3f, 0);

            // TODO(orendain): Fix so that ray interacts with entire object.
            this.gameObject.AddComponent<BoxCollider>();

            isInitialized = true;
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
            MelonLogger.Msg($"[RoomListUI] Retrieved {cachedRooms.Count} rooms.");

            var roomListPanelContainer = roomListPanel.Reinitialize(cachedRooms.Values.ToList());
            roomListPanelContainer.transform.SetParent(this.transform, worldPositionStays: false);
            roomListPanelContainer.transform.localPosition = new Vector3(0, -1, 0);
        }
    }
}
