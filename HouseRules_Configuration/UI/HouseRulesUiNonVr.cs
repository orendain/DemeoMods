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
            transform.SetParent(_resourceTable.AnchorDesktopMainMenu.transform, worldPositionStays: false);

            var rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 1);

            var paper = new GameObject("PaperBackground");
            paper.transform.SetParent(transform, worldPositionStays: false);
            paper.AddComponent<Image>().sprite = _resourceTable.PaperDecorated;
            paper.GetComponent<RectTransform>().sizeDelta = new Vector2(1576, 876);

            var headerText = _elementCreator.CreateMenuHeaderText("HouseRules");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            headerText.transform.localPosition = new Vector2(0, 310f);

            var selectionPanel = _rulesetPanel.Panel;
            selectionPanel.transform.SetParent(transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector2(0, 230f);

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector2(-615, -400);

            if (ConfigurationMod.IsUpdateAvailable)
            {
                var updateText = _elementCreator.CreateNormalText("NEW UPDATE AVAILABLE!");
                updateText.transform.SetParent(transform, worldPositionStays: false);
                updateText.transform.localPosition = new Vector2(-480f, -390);
            }

            var navigation = CreateNavigationButton();
            navigation.transform.SetParent(_resourceTable.AnchorNavigationBar.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector2(725, -30);

            gameObject.SetActive(_pageVisible);
        }

        private GameObject CreateNavigationButton()
        {
            var container = new GameObject("HouseRulesNavigation");

            var button = _elementCreator.CreateButton(TogglePage);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector2(1.75f, 0.65f);

            var text = _elementCreator.CreateText("HouseRules", Color.white, NonVrElementCreator.NormalFontSize);
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
    }
}
