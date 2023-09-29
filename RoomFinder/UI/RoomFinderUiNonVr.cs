namespace RoomFinder.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.Ui.LobbyMenu;
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
        private bool _pageVisible;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
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
            _roomListPanel = RoomListPanelNonVr.NewInstance(_elementCreator, RefreshRoomList);

            Initialize();
            RoomFinderBase.LogDebug("Initialization complete.");
        }

        private void Update()
        {
            if (!RoomFinderBase.SharedState.IsRefreshingRoomList)
            {
                return;
            }

            if (!RoomFinderBase.SharedState.HasRoomListUpdated)
            {
                return;
            }

            RoomFinderBase.SharedState.IsRefreshingRoomList = false;
            RoomFinderBase.SharedState.HasRoomListUpdated = false;
            PopulateRoomList();
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

        private static void RefreshRoomList()
        {
            RoomFinderBase.SharedState.IsRefreshingRoomList = true;
            var lobbyMenuController = Traverse
                .Create(RoomFinderBase.SharedState.GameContext.gameStateMachine.lobby)
                .Field<LobbyMenuController>("lobbyMenuController")
                .Value;
            var lobbyMenuContext = Traverse
                .Create(lobbyMenuController)
                .Field<LobbyMenu.ILobbyMenuContext>("lobbyMenuContext")
                .Value;

            lobbyMenuContext.QuickPlay(
                LevelSequence.GameType.Invalid,
                LevelSequence.ControlType.OneHero,
                matchMakeAnyGame: true,
                onError: null);
        }

        private void PopulateRoomList()
        {
            var unfilteredRooms =
                Traverse.Create(RoomFinderBase.SharedState.LobbyMatchmakingController)
                    .Field<List<RoomInfo>>("roomList").Value;

            var isRoomValidMethod =
                Traverse.Create(RoomFinderBase.SharedState.LobbyMatchmakingController)
                    .Method("IsRoomValidWithCurrentConfiguration", new[] { typeof(RoomInfo) });

            var filteredRooms =
                unfilteredRooms.Where(info => isRoomValidMethod.GetValue<bool>(info))
                    .Select(Room.Parse)
                    .ToList();

            RoomFinderBase.LogInfo($"Found {unfilteredRooms.Count} total rooms.");
            RoomFinderBase.LogInfo($"Listing {filteredRooms.Count} available rooms.");

            _roomListPanel.UpdateRooms(filteredRooms);
        }
    }
}
