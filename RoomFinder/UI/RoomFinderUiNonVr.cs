﻿namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using Common.UI;
    using Common.UI.Element;
    using UnityEngine;
    using UnityEngine.UI;

    internal class RoomFinderUiNonVr : MonoBehaviour
    {
        private NonVrResourceTable _resourceTable;
        private NonVrElementCreator _elementCreator;
        private RoomListPanelNonVr _roomListPanel;
        private bool _pageVisible;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
            RoomManager.RoomListUpdated += OnRoomListUpdated;
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!NonVrElementCreator.IsReady())
            {
                RoomFinderBase.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderBase.LogDebug("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = NonVrResourceTable.Instance();
            _elementCreator = NonVrElementCreator.Instance();
            _roomListPanel = RoomListPanelNonVr.NewInstance(_elementCreator, RoomManager.RefreshRoomList);

            Initialize();
            RoomFinderBase.LogDebug("Initialization complete.");
        }

        private void Initialize()
        {
            transform.SetParent(_resourceTable.AnchorDesktopMainMenu.transform, worldPositionStays: false);

            var rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 1);

            var paper = new GameObject("PaperBackground");
            paper.transform.SetParent(transform, worldPositionStays: false);
            paper.AddComponent<Image>().sprite = _resourceTable.PaperDecorated;
            paper.GetComponent<RectTransform>().sizeDelta = new Vector2(1576, 876);

            var headerText = _elementCreator.CreateMenuHeaderText("RoomFinder");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            headerText.transform.localPosition = new Vector2(0, 310f);

            var selectionPanel = _roomListPanel.Panel;
            selectionPanel.transform.SetParent(transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector2(0, 230f);

            var versionText = _elementCreator.CreateNormalText($"v{RoomFinderBase.ModVersion}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector2(-615, -400);

            var navigation = CreateNavigationButton();
            navigation.transform.SetParent(_resourceTable.AnchorNavigationBar.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector2(725, -80);

            gameObject.SetActive(_pageVisible);
        }

        private GameObject CreateNavigationButton()
        {
            var container = new GameObject("RoomFinderNavigation");

            var button = _elementCreator.CreateButton(TogglePage);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector2(1.75f, 0.65f);

            var text = _elementCreator.CreateText("RoomFinder", Color.white, NonVrElementCreator.NormalFontSize);
            text.transform.SetParent(container.transform, worldPositionStays: false);
            text.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        public void TogglePage()
        {
            _pageVisible = !_pageVisible;
            gameObject.SetActive(_pageVisible);
        }

        public void HideDesktopPages()
        {
            foreach (Transform child in _resourceTable.AnchorDesktopPages.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void OnRoomListUpdated(List<Room> rooms)
        {
            RoomFinderBase.LogInfo($"Listing {rooms.Count} available rooms.");
            _roomListPanel.UpdateRooms(rooms);
        }
    }
}
