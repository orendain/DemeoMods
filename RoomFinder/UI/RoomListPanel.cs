namespace RoomFinder.UI
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
        private readonly UiHelper _uiHelper;
        private Func<RoomListEntry, object> _sortOrder;
        private bool _shouldHideFullRooms;
        private bool _isDescendingOrder;
        private List<RoomListEntry> _originalRooms;
        private List<RoomListEntry> _rooms;

        internal GameObject GameObject { get; }

        internal static RoomListPanel NewInstance(UiHelper uiHelper)
        {
            return new RoomListPanel(uiHelper, new GameObject("RoomListPanel"));
        }

        private RoomListPanel(UiHelper uiHelper, GameObject panel)
        {
            this._uiHelper = uiHelper;
            this.GameObject = panel;
            this._sortOrder = r => r;
            this._shouldHideFullRooms = false;
            this._isDescendingOrder = false;

            this._originalRooms = new List<RoomListEntry>();
            this._rooms = this._originalRooms;
        }

        internal void SetRooms(IEnumerable<RoomInfo> rooms)
        {
            _originalRooms = rooms.Select(RoomListEntry.Parse).ToList();
            _rooms = _originalRooms;

            Render();
        }

        private void Render()
        {
            foreach (Transform child in GameObject.transform)
            {
                Object.Destroy(child.gameObject);
            }

            RenderHeader();

            for (var i = 0; i < _rooms.Count; i++)
            {
                RenderRoomRow(_rooms[i], i);
            }
        }

        private void RenderHeader()
        {
            var headerContainer = new GameObject("Header");
            headerContainer.transform.SetParent(GameObject.transform, worldPositionStays: false);

            var toggleHideFull = CreateHideButton("Hide Full?", ToggleHideFull);
            toggleHideFull.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            toggleHideFull.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            toggleHideFull.transform.localPosition = new Vector3(2.5f, 1, 0);

            var sortLabel = _uiHelper.CreateLabelText("Sort by:");
            sortLabel.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            sortLabel.transform.localPosition = new Vector3(-3f, 0, 0);

            var gameButton = CreateSortButton("Game", () => SortListBy(r => r.GameType));
            gameButton.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            gameButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            gameButton.transform.localPosition = new Vector3(-0.5f, 0, 0);

            var floorButton = CreateSortButton("Floor", () => SortListBy(r => r.Floor));
            floorButton.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            floorButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            floorButton.transform.localPosition = new Vector3(1.5f, 0, 0);

            var playersButton = CreateSortButton("Players", () => SortListBy(r => r.CurrentPlayers));
            playersButton.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            playersButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            playersButton.transform.localPosition = new Vector3(3.5f, 0, 0);
        }

        private GameObject CreateHideButton(string text, Action action)
        {
            var container = new GameObject(text);

            var button = _uiHelper.CreateButton(action);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(1.2f, 1, 1);

            var buttonText = _uiHelper.CreateText(text, Color.white, UiHelper.DefaultButtonFontSize);
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            return container;
        }

        private GameObject CreateSortButton(string text, Action action)
        {
            var container = new GameObject(text);

            var button = _uiHelper.CreateButton(action);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(0.75f, 1, 1);

            var buttonText = _uiHelper.CreateText(text, Color.white, UiHelper.DefaultButtonFontSize);
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            return container;
        }

        private void SortListBy(Func<RoomListEntry, object> sortOrder)
        {
            if (_sortOrder == sortOrder)
            {
                _isDescendingOrder = !_isDescendingOrder;
            }

            _sortOrder = sortOrder;
            ResortRooms();
        }

        private void ToggleHideFull()
        {
            _shouldHideFullRooms = !_shouldHideFullRooms;
            ResortRooms();
        }

        private void ResortRooms()
        {
            _rooms = _shouldHideFullRooms
                ? _originalRooms.Where(r => r.CurrentPlayers != r.MaxPlayers).ToList()
                : _originalRooms;

            _rooms = _isDescendingOrder
                ? _rooms.OrderByDescending(_sortOrder).ToList()
                : _rooms.OrderBy(_sortOrder).ToList();

            Render();
        }

        private void RenderRoomRow(RoomListEntry room, int row)
        {
            var yOffset = (1 + row) * -1f;
            var roomRowContainer = new GameObject($"Row{row}");
            roomRowContainer.transform.SetParent(GameObject.transform, worldPositionStays: false);
            roomRowContainer.transform.localPosition = new Vector3(0, yOffset, 0);

            if (room.GameType == LevelSequence.GameType.Invalid || room.Floor < 0)
            {
                return;
            }

            var joinButton = _uiHelper.CreateButton(JoinRoomAction(room.Name));
            joinButton.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            joinButton.transform.localScale = new Vector3(0.4f, 0.7f, 0.7f);
            joinButton.transform.localPosition = new Vector3(-3f, 0, 0);

            var joinText = _uiHelper.CreateText(room.Name, Color.white, UiHelper.DefaultLabelFontSize);
            joinText.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            joinText.transform.localPosition = new Vector3(-3f, 0, 0);

            var gameName = room.GameType.ToString();
            var gameLabel = _uiHelper.CreateLabelText(gameName);
            gameLabel.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            gameLabel.transform.localPosition = new Vector3(-0.4f, 0, 0);

            var floorLabel = _uiHelper.CreateLabelText(room.Floor.ToString());
            floorLabel.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            floorLabel.transform.localPosition = new Vector3(1.75f, 0, 0);

            var playersText = $"{room.CurrentPlayers}/{room.MaxPlayers}";
            var playersLabel = _uiHelper.CreateLabelText(playersText);
            playersLabel.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            playersLabel.transform.localPosition = new Vector3(3.25f, 0, 0);
        }

        private static Action JoinRoomAction(string roomCode)
        {
            return () =>
            {
                RoomFinderMod.Logger.Msg($"Joining room [{roomCode}].");
                Traverse.Create(RoomFinderMod.GameContextState.LobbyMenuController).Method("JoinGame", roomCode, true)
                    .GetValue();
            };
        }
    }
}
