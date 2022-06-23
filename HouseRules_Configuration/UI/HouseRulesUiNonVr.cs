namespace HouseRules.Configuration.UI
{
    using System.Collections;
    using Common.UI;
    using Common.UI.Element;
    using UnityEngine;
    using UnityEngine.UI;

    internal class HouseRulesUiNonVr : MonoBehaviour
    {
        private NonVrResourceTable _resourceTable;
        private IElementCreator _elementCreator;
        private RulesetSelectionPanelNonVr _rulesetPanel;
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
                ConfigurationMod.Logger.Msg("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            ConfigurationMod.Logger.Msg("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = NonVrResourceTable.Instance();
            _elementCreator = NonVrElementCreator.Instance();
            _rulesetPanel = RulesetSelectionPanelNonVr.NewInstance(HR.Rulebook, _elementCreator);

            Initialize();
            ConfigurationMod.Logger.Msg("Initialization complete.");
        }

        private void Initialize()
        {
            var navigation = CreateNavigationButton();
            navigation.transform.SetParent(_resourceTable.AnchorNavigationBar.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector3(725, -30);

            _page = CreatePage();
            _page.transform.SetParent(_resourceTable.AnchorDesktopMainMenu.transform, worldPositionStays: false);
            _page.SetActive(_pageVisible);
        }

        private GameObject CreateNavigationButton()
        {
            var container = new GameObject("HouseRules");

            var button = _elementCreator.CreateButton(TogglePage);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector2(1.75f, 0.65f);

            var text = _elementCreator.CreateText("HouseRules", Color.white, NonVrElementCreator.NormalFontSize);
            text.transform.SetParent(container.transform, worldPositionStays: false);
            text.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        private GameObject CreatePage()
        {
            var page = new GameObject("HouseRulesPage");
            var pageRectTransform = page.AddComponent<RectTransform>();
            pageRectTransform.pivot = new Vector2(0.5f, 1);

            var paperContainer = new GameObject("paper");
            paperContainer.transform.SetParent(page.transform, worldPositionStays: false);
            paperContainer.AddComponent<Image>().sprite = _resourceTable.PaperDecorated;
            paperContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(1576, 876);

            var title = _elementCreator.CreateText("HouseRules", NonVrElementCreator.ColorBrown, 36);
            title.transform.SetParent(page.transform, worldPositionStays: false);
            title.transform.localPosition = new Vector2(0, 310f);

            var selectionPanel = _rulesetPanel.Panel;
            selectionPanel.transform.SetParent(page.transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector2(0, 230f);

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(page.transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector2(-615, -400);

            if (ConfigurationMod.IsUpdateAvailable)
            {
                var updateText = _elementCreator.CreateNormalText("NEW UPDATE AVAILABLE!");
                updateText.transform.SetParent(page.transform, worldPositionStays: false);
                updateText.transform.localPosition = new Vector2(-480f, -390);
            }

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
    }
}
