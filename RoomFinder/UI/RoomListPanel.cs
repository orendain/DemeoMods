﻿namespace RoomFinder.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Common.UI;
    using HarmonyLib;
    using Photon.Realtime;
    using UnityEngine;
    using Object = UnityEngine.Object;

    internal class RoomListPanel
    {
        private const int MaxRoomsPerPage = 14;

        private readonly UiHelper _uiHelper;
        private readonly PageStack _pageStack;
        private Func<RoomListEntry, object> _sortOrder;
        private bool _isDescendingOrder;
        private List<RoomListEntry> _rooms;

        internal GameObject GameObject { get; }

        internal static RoomListPanel NewInstance(UiHelper uiHelper)
        {
            return new RoomListPanel(
                uiHelper,
                new GameObject("RoomListPanel"),
                PageStack.NewInstance(uiHelper));
        }

        private RoomListPanel(UiHelper uiHelper, GameObject panel,PageStack pageStack)
        {
            this._uiHelper = uiHelper;
            this._pageStack = pageStack;
            this.GameObject = panel;
            this._sortOrder = r => r.CurrentPlayers;
            this._isDescendingOrder = true;

            this._rooms = new List<RoomListEntry>();
        }

        internal void SetRooms(IEnumerable<RoomInfo> rooms)
        {
            _rooms = rooms.Select(RoomListEntry.Parse).ToList();
            SortRooms();
            Render();
        }

        private void Render()
        {
            foreach (Transform child in GameObject.transform)
            {
                Object.Destroy(child.gameObject);
            }

            var header = CreateHeader();
            header.transform.SetParent(GameObject.transform, worldPositionStays: false);

            var roomPartitions = PartitionRooms();
            var roomPages = roomPartitions.Select(CreatePage).ToList();
            roomPages.ForEach(_pageStack.AddPage);

            var pageNavigation = _pageStack.NavigationPanel;
            pageNavigation.transform.SetParent(GameObject.transform, worldPositionStays: false);
            pageNavigation.transform.localPosition = new Vector3(0, -17f, 0);

            var rooms = new GameObject("Rooms");
            rooms.transform.SetParent(GameObject.transform, worldPositionStays: false);
        }

        private GameObject CreateHeader()
        {
            var container = new GameObject("Header");

            var sortLabel = _uiHelper.CreateLabelText("Sort by:");
            sortLabel.transform.SetParent(container.transform, worldPositionStays: false);
            sortLabel.transform.localPosition = new Vector3(-3f, 0, UiHelper.DefaultTextZShift);

            var gameButton = CreateSortButton("Game", () => SetSortOrderAndApply(r => r.GameType));
            gameButton.transform.SetParent(container.transform, worldPositionStays: false);
            gameButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            gameButton.transform.localPosition = new Vector3(-0.5f, 0, 0);

            var floorButton = CreateSortButton("Floor", () => SetSortOrderAndApply(r => r.Floor));
            floorButton.transform.SetParent(container.transform, worldPositionStays: false);
            floorButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            floorButton.transform.localPosition = new Vector3(1.5f, 0, 0);

            var playersButton = CreateSortButton("Players", () => SetSortOrderAndApply(r => r.CurrentPlayers));
            playersButton.transform.SetParent(container.transform, worldPositionStays: false);
            playersButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            playersButton.transform.localPosition = new Vector3(3.5f, 0, 0);

            return container;
        }

        /// <summary>
        /// Partition rooms into groups based on the maximum allowed rooms per page.
        /// </summary>
        private IEnumerable<List<RoomListEntry>> PartitionRooms()
        {
            return FilterValidRooms(_rooms)
                .Select((value, index) => new { group = index / MaxRoomsPerPage, value })
                .GroupBy(pair => pair.group)
                .Select(group => group.Select(g => g.value).ToList());
        }

        private static IEnumerable<RoomListEntry> FilterValidRooms(IEnumerable<RoomListEntry> rooms)
        {
            return rooms.Where(room => room.GameType != LevelSequence.GameType.Invalid).Where(room => room.Floor >= 0);
        }

        private GameObject CreatePage(IReadOnlyCollection<RoomListEntry> rooms)
        {
            var container = new GameObject("Rooms");
            container.transform.SetParent(GameObject.transform, worldPositionStays: false);
            container.transform.localPosition = new Vector3(0, -1.5f, 0);

            for (var i = 0; i < rooms.Count; i++)
            {
                var yOffset = i * -1f;
                var roomRow = CreateRoomRow(rooms.ElementAt(i));
                roomRow.transform.SetParent(container.transform, worldPositionStays: false);
                roomRow.transform.localPosition = new Vector3(0, yOffset, 0);
            }

            return container;
        }

        private GameObject CreateSortButton(string text, Action action)
        {
            var container = new GameObject(text);

            var button = _uiHelper.CreateButton(action);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(0.55f, 0.9f, 0.9f);
            button.transform.localPosition = new Vector3(0, 0, UiHelper.DefaultButtonZShift);

            var buttonText = _uiHelper.CreateText(text, Color.white, UiHelper.DefaultButtonFontSize);
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            buttonText.transform.localPosition = new Vector3(0, 0, UiHelper.DefaultButtonZShift + UiHelper.DefaultTextZShift);

            return container;
        }

        private void SetSortOrderAndApply(Func<RoomListEntry, object> sortOrder)
        {
            if (_sortOrder == sortOrder)
            {
                _isDescendingOrder = !_isDescendingOrder;
            }

            _sortOrder = sortOrder;
            SortRooms();
            Render();
        }

        private void SortRooms()
        {
            _rooms = _isDescendingOrder
                ? _rooms.OrderByDescending(_sortOrder).ToList()
                : _rooms.OrderBy(_sortOrder).ToList();
        }

        private GameObject CreateRoomRow(RoomListEntry room)
        {
            var container = new GameObject(room.Name);

            var joinButton = _uiHelper.CreateButton(JoinRoomAction(room.Name));
            joinButton.transform.SetParent(container.transform, worldPositionStays: false);
            joinButton.transform.localScale = new Vector3(0.32f, 0.45f, 0.45f);
            joinButton.transform.localPosition = new Vector3(-3f, 0, UiHelper.DefaultButtonZShift);

            var joinText = _uiHelper.CreateText(room.Name, Color.white, UiHelper.DefaultLabelFontSize);
            joinText.transform.SetParent(container.transform, worldPositionStays: false);
            joinText.transform.localPosition = new Vector3(-3f, 0, UiHelper.DefaultTextZShift);

            var gameLabel = _uiHelper.CreateLabelText(room.GameType.ToString());
            gameLabel.transform.SetParent(container.transform, worldPositionStays: false);
            gameLabel.transform.localPosition = new Vector3(-0.4f, 0, UiHelper.DefaultTextZShift);

            var floorLabel = _uiHelper.CreateLabelText(room.Floor.ToString());
            floorLabel.transform.SetParent(container.transform, worldPositionStays: false);
            floorLabel.transform.localPosition = new Vector3(1.75f, 0, UiHelper.DefaultTextZShift);

            var playersLabel = _uiHelper.CreateLabelText($"{room.CurrentPlayers}/{room.MaxPlayers}");
            playersLabel.transform.SetParent(container.transform, worldPositionStays: false);
            playersLabel.transform.localPosition = new Vector3(3.25f, 0, UiHelper.DefaultTextZShift);

            return container;
        }

        private static Action JoinRoomAction(string roomCode)
        {
            return () =>
            {
                RoomFinderMod.Logger.Msg($"Joining room [{roomCode}].");
                Traverse.Create(RoomFinderMod.ModState.GameContext.gameStateMachine.lobby.GetLobbyMenuController)
                    .Method("JoinGame", roomCode, true)
                    .GetValue();
            };
        }
    }
}
