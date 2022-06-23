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
    using UnityEngine.UI;

    internal class RoomFinderUiNonVr : MonoBehaviour
    {
        private NonVrResourceTable _resourceTable;
        private NonVrElementCreator _elementCreator;
        private RoomListPanelNonVr _roomListPanel;
        private GameObject _page;
        private bool _pageVisible;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!NonVrElementCreator.IsReady())
            {
                RoomFinderMod.Logger.Msg("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderMod.Logger.Msg("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = NonVrResourceTable.Instance();
            _elementCreator = NonVrElementCreator.Instance();
            _roomListPanel = RoomListPanelNonVr.NewInstance(_elementCreator, RefreshRoomList);

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
            var navigation = CreateNavigationButton();
            navigation.transform.SetParent(_resourceTable.AnchorNavigationBar.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector3(725, -80);

            _page = CreatePage();
            _page.transform.SetParent(_resourceTable.AnchorDesktopMainMenu.transform, worldPositionStays: false);
            _page.SetActive(_pageVisible);
        }

        private GameObject CreateNavigationButton()
        {
            var container = new GameObject("RoomFinder");

            var button = _elementCreator.CreateButton(TogglePage);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector2(1.75f, 0.65f);

            var text = _elementCreator.CreateText("RoomFinder", Color.white, NonVrElementCreator.DefaultLabelFontSize);
            text.transform.SetParent(container.transform, worldPositionStays: false);
            text.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        private GameObject CreatePage()
        {
            var page = new GameObject("RoomFinderPage");
            var pageRectTransform = page.AddComponent<RectTransform>();
            pageRectTransform.pivot = new Vector2(0.5f, 1);

            var paperContainer = new GameObject("paper");
            paperContainer.transform.SetParent(page.transform, worldPositionStays: false);
            paperContainer.AddComponent<Image>().sprite = _resourceTable.PaperDecorated;
            paperContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(1576, 876);

            var title = _elementCreator.CreateText("RoomFinder", _resourceTable.ColorBrown, 36);
            title.transform.SetParent(page.transform, worldPositionStays: false);
            title.transform.localPosition = new Vector2(0, 310f);

            var selectionPanel = _roomListPanel.Panel;
            selectionPanel.transform.SetParent(page.transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector2(0, 230f);

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(page.transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector2(-615, -400);

            return page;
        }

        public void TogglePage()
        {
            _pageVisible = !_pageVisible;
            _page.SetActive(_pageVisible);
        }

        public void HideDesktopPages()
        {
            foreach (Transform child in _resourceTable.AnchorDesktopPages.transform)
            {
                child.gameObject.SetActive(false);
            }
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
