﻿namespace RoomFinder.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Common.UI;
    using Common.UI.Element;
    using HarmonyLib;
    using UnityEngine;
    using Object = UnityEngine.Object;

    internal class RoomListPanel
    {
        private const int MaxRoomsPerPage = 14;

        private readonly VrElementCreator _elementCreator;
        private readonly Action _onRefresh;
        private readonly PageStack _pageStack;
        private Func<Room, object> _sortOrder;
        private bool _isDescendingOrder;
        private IEnumerable<Room> _rooms;
        private GameObject _roomPages;

        internal GameObject Panel { get; }

        internal static RoomListPanel NewInstance(VrElementCreator uiHelper, Action onRefresh)
        {
            return new RoomListPanel(
                uiHelper,
                onRefresh,
                PageStack.NewInstance(uiHelper),
                new GameObject("RoomListPanel"));
        }

        private RoomListPanel(VrElementCreator elementCreator, Action onRefresh, PageStack pageStack, GameObject panel)
        {
            _elementCreator = elementCreator;
            _onRefresh = onRefresh;
            _pageStack = pageStack;
            Panel = panel;

            _sortOrder = r => r.CurrentPlayers;
            _isDescendingOrder = true;
            _rooms = new List<Room>();

            Draw();
        }

        internal void UpdateRooms(IEnumerable<Room> rooms)
        {
            _rooms = rooms;
            FilterValidRooms();
            SortRooms();
            DrawRoomPages();
        }

        private void Draw()
        {
            foreach (Transform child in Panel.transform)
            {
                Object.Destroy(child.gameObject);
            }

            var header = CreateHeader();
            header.transform.SetParent(Panel.transform, worldPositionStays: false);

            _roomPages = new GameObject("RoomPages");
            _roomPages.transform.SetParent(Panel.transform, worldPositionStays: false);
            _roomPages.transform.localPosition = new Vector3(0, -3f, 0);

            var pageNavigation = CreateNavigation();
            pageNavigation.transform.SetParent(Panel.transform, worldPositionStays: false);
            pageNavigation.transform.localPosition = new Vector3(0, -18.2f, 0);

            DrawRoomPages();
        }

        private GameObject CreateNavigation()
        {
            var container = new GameObject("PageStackNavigation");

            _pageStack.Navigation.PageStatus.transform.SetParent(container.transform, worldPositionStays: false);

            _pageStack.Navigation.PreviousButton.transform.SetParent(container.transform, worldPositionStays: false);
            _pageStack.Navigation.PreviousButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            _pageStack.Navigation.PreviousButton.transform.localPosition =
                new Vector3(-2.5f, 0, VrElementCreator.DefaultButtonZShift);

            _pageStack.Navigation.PreviousButtonText.transform.SetParent(
                container.transform,
                worldPositionStays: false);
            _pageStack.Navigation.PreviousButtonText.transform.localPosition = new Vector3(
                -2.5f,
                0,
                VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            _pageStack.Navigation.NextButton.transform.SetParent(container.transform, worldPositionStays: false);
            _pageStack.Navigation.NextButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            _pageStack.Navigation.NextButton.transform.localPosition =
                new Vector3(2.5f, 0, VrElementCreator.DefaultButtonZShift);

            _pageStack.Navigation.NextButtonText.transform.SetParent(container.transform, worldPositionStays: false);
            _pageStack.Navigation.NextButtonText.transform.localPosition = new Vector3(
                2.5f,
                0,
                VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            return container;
        }

        private void DrawRoomPages()
        {
            _pageStack.Clear();

            foreach (Transform child in _roomPages.transform)
            {
                Object.Destroy(child.gameObject);
            }

            var roomPartitions = PartitionRooms();
            var roomPages = roomPartitions.Select(CreatePage).ToList();
            foreach (var page in roomPages)
            {
                page.transform.SetParent(_roomPages.transform, worldPositionStays: false);
                _pageStack.AddPage(page);
            }
        }

        private GameObject CreateHeader()
        {
            var container = new GameObject("Header");

            var refreshButton = _elementCreator.CreateButton(_onRefresh);
            refreshButton.transform.SetParent(container.transform, worldPositionStays: false);
            refreshButton.transform.localPosition = new Vector3(0, 0, VrElementCreator.DefaultButtonZShift);
            refreshButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            var refreshText = _elementCreator.CreateButtonText("Refresh");
            refreshText.transform.SetParent(container.transform, worldPositionStays: false);
            refreshText.transform.localPosition =
                new Vector3(0, 0, VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            var sortHeader = CreateSortHeader();
            sortHeader.transform.SetParent(container.transform, worldPositionStays: false);
            sortHeader.transform.localPosition = new Vector3(0, -1.75f, 0);

            return container;
        }

        private GameObject CreateSortHeader()
        {
            var container = new GameObject("SortHeader");

            var sortLabel = _elementCreator.CreateNormalText("Sort by:");
            sortLabel.transform.SetParent(container.transform, worldPositionStays: false);
            sortLabel.transform.localPosition = new Vector3(-3f, 0, VrElementCreator.DefaultTextZShift);

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
        private IEnumerable<List<Room>> PartitionRooms()
        {
            return _rooms
                .Select((value, index) => new { group = index / MaxRoomsPerPage,  value })
                .GroupBy(pair => pair.group)
                .Select(group => group.Select(g => g.value).ToList());
        }

        private GameObject CreatePage(IReadOnlyCollection<Room> rooms)
        {
            var container = new GameObject("RoomPage");

            for (var i = 0; i < rooms.Count; i++)
            {
                var yOffset = i * -1f;
                var roomRow = CreateRoomRow(rooms.ElementAt(i));
                roomRow.transform.SetParent(container.transform, worldPositionStays: false);
                roomRow.transform.localPosition = new Vector3(0, yOffset, 0);
            }

            return container;
        }

        private GameObject CreateRoomRow(Room room)
        {
            var container = new GameObject(room.Name);

            var joinButton = _elementCreator.CreateButton(JoinRoomAction(room.Name));
            joinButton.transform.SetParent(container.transform, worldPositionStays: false);
            joinButton.transform.localScale = new Vector3(0.32f, 0.45f, 0.45f);
            joinButton.transform.localPosition = new Vector3(-3f, 0, VrElementCreator.DefaultButtonZShift);

            var joinText = _elementCreator.CreateText(room.Name, Color.white, VrElementCreator.DefaultLabelFontSize);
            joinText.transform.SetParent(container.transform, worldPositionStays: false);
            joinText.transform.localPosition = new Vector3(-3f, 0, VrElementCreator.DefaultTextZShift);

            var gameLabel = _elementCreator.CreateNormalText(room.GameType.ToString());
            gameLabel.transform.SetParent(container.transform, worldPositionStays: false);
            gameLabel.transform.localPosition = new Vector3(-0.4f, 0, VrElementCreator.DefaultTextZShift);

            var floorLabel = _elementCreator.CreateNormalText(room.Floor.ToString());
            floorLabel.transform.SetParent(container.transform, worldPositionStays: false);
            floorLabel.transform.localPosition = new Vector3(1.75f, 0, VrElementCreator.DefaultTextZShift);

            var playersLabel = _elementCreator.CreateNormalText($"{room.CurrentPlayers}/{room.MaxPlayers}");
            playersLabel.transform.SetParent(container.transform, worldPositionStays: false);
            playersLabel.transform.localPosition = new Vector3(3.25f, 0, VrElementCreator.DefaultTextZShift);

            return container;
        }

        private GameObject CreateSortButton(string text, Action action)
        {
            var container = new GameObject(text);

            var button = _elementCreator.CreateButton(action);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(0.55f, 0.9f, 0.9f);
            button.transform.localPosition = new Vector3(0, 0, VrElementCreator.DefaultButtonZShift);

            var buttonText = _elementCreator.CreateText(text, Color.white, _elementCreator.DefaultButtonFontSize());
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            buttonText.transform.localPosition =
                new Vector3(0, 0, VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            return container;
        }

        private void SetSortOrderAndApply(Func<Room, object> sortOrder)
        {
            if (_sortOrder == sortOrder)
            {
                _isDescendingOrder = !_isDescendingOrder;
            }

            _sortOrder = sortOrder;
            SortRooms();
            DrawRoomPages();
        }

        /// <summary>
        /// Filter out invalid rooms.
        /// </summary>
        private void FilterValidRooms()
        {
            _rooms = _rooms
                .Where(room => room.GameType != LevelSequence.GameType.Invalid)
                .Where(room => room.Floor >= 0);
        }

        /// <summary>
        /// Sort rooms based on the sort order preference.
        /// </summary>
        private void SortRooms()
        {
            _rooms = _isDescendingOrder
                ? _rooms.OrderByDescending(_sortOrder).ToList()
                : _rooms.OrderBy(_sortOrder).ToList();
        }

        private static Action JoinRoomAction(string roomCode)
        {
            return () =>
            {
                RoomFinderMod.Logger.Msg($"Joining room [{roomCode}].");
                Traverse.Create(RoomFinderMod.SharedState.GameContext.gameStateMachine.lobby.GetLobbyMenuController)
                    .Method("JoinGame", roomCode, true)
                    .GetValue();
            };
        }
    }
}
