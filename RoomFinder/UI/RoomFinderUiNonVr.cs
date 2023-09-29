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
<<<<<<< HEAD
                RoomFinderCore.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderCore.LogDebug("UI dependencies ready. Proceeding with initialization.");
=======
                RoomFinderBase.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            RoomFinderBase.LogDebug("UI dependencies ready. Proceeding with initialization.");
>>>>>>> main

            _resourceTable = NonVrResourceTable.Instance();
            _elementCreator = NonVrElementCreator.Instance();
            _roomListPanel = RoomListPanelNonVr.NewInstance(_elementCreator, RefreshRoomList);

            Initialize();
<<<<<<< HEAD
            RoomFinderCore.LogDebug("Initialization complete.");
=======
            RoomFinderBase.LogDebug("Initialization complete.");
>>>>>>> main
        }

        private void Update()
        {
<<<<<<< HEAD
            if (!RoomFinderCore.SharedState.IsRefreshingRoomList)
=======
            if (!RoomFinderBase.SharedState.IsRefreshingRoomList)
>>>>>>> main
            {
                return;
            }

<<<<<<< HEAD
            if (!RoomFinderCore.SharedState.HasRoomListUpdated)
=======
            if (!RoomFinderBase.SharedState.HasRoomListUpdated)
>>>>>>> main
            {
                return;
            }

<<<<<<< HEAD
            RoomFinderCore.SharedState.IsRefreshingRoomList = false;
            RoomFinderCore.SharedState.HasRoomListUpdated = false;
=======
            RoomFinderBase.SharedState.IsRefreshingRoomList = false;
            RoomFinderBase.SharedState.HasRoomListUpdated = false;
>>>>>>> main
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

<<<<<<< HEAD
            var versionText = _elementCreator.CreateNormalText($"v{RoomFinderCore.ModVersion}");
=======
            var versionText = _elementCreator.CreateNormalText($"v{RoomFinderBase.ModVersion}");
>>>>>>> main
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
<<<<<<< HEAD
            RoomFinderCore.SharedState.IsRefreshingRoomList = true;
            var lobbyMenuController = Traverse
                .Create(RoomFinderCore.SharedState.GameContext.gameStateMachine.lobby)
=======
            RoomFinderBase.SharedState.IsRefreshingRoomList = true;
            var lobbyMenuController = Traverse
                .Create(RoomFinderBase.SharedState.GameContext.gameStateMachine.lobby)
>>>>>>> main
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
<<<<<<< HEAD
                Traverse.Create(RoomFinderCore.SharedState.LobbyMatchmakingController)
                    .Field<List<RoomInfo>>("roomList").Value;

            var isRoomValidMethod =
                Traverse.Create(RoomFinderCore.SharedState.LobbyMatchmakingController)
=======
                Traverse.Create(RoomFinderBase.SharedState.LobbyMatchmakingController)
                    .Field<List<RoomInfo>>("roomList").Value;

            var isRoomValidMethod =
                Traverse.Create(RoomFinderBase.SharedState.LobbyMatchmakingController)
>>>>>>> main
                    .Method("IsRoomValidWithCurrentConfiguration", new[] { typeof(RoomInfo) });

            var filteredRooms =
                unfilteredRooms.Where(info => isRoomValidMethod.GetValue<bool>(info))
                    .Select(Room.Parse)
                    .ToList();

<<<<<<< HEAD
            RoomFinderCore.LogInfo($"Found {unfilteredRooms.Count} total rooms.");
            RoomFinderCore.LogInfo($"Listing {filteredRooms.Count} available rooms.");
=======
            RoomFinderBase.LogInfo($"Found {unfilteredRooms.Count} total rooms.");
            RoomFinderBase.LogInfo($"Listing {filteredRooms.Count} available rooms.");
>>>>>>> main

            _roomListPanel.UpdateRooms(filteredRooms);
        }
    }
}
