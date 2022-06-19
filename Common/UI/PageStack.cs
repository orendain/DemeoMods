namespace Common.UI
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    internal class PageStack
    {
        private readonly UiHelper _uiHelper;
        private readonly List<GameObject> _pages;
        private readonly TextMeshPro _statusText;
        private int _currentPageIndex;

        internal GameObject NavigationPanel { get; }

        public static PageStack NewInstance(UiHelper uiHelper)
        {
            return new PageStack(uiHelper);
        }

        private PageStack(UiHelper uiHelper)
        {
            _uiHelper = uiHelper;
            _pages = new List<GameObject>();
            _currentPageIndex = 0;
            NavigationPanel = CreateNavigation();

            _statusText = NavigationPanel.GetComponentInChildren<TextMeshPro>();
            UpdatePageStatus();
        }

        /// <summary>
        /// Adds the specified page to the stack navigation order.
        /// </summary>
        public void AddPage(GameObject page)
        {
            _pages.Add(page);
            UpdatePageStatus();
            UpdatePageVisibility();
        }

        /// <summary>
        /// Removes all pages from the stack.
        /// </summary>
        /// <remarks>
        /// Note that this stops tracking all current pages, but does not explicitly destroy them.
        /// </remarks>
        public void Clear()
        {
            _pages.Clear();
            _currentPageIndex = 0;
            UpdatePageStatus();
        }

        private void OnPreviousPageClick()
        {
            AdvancePageIndex(-1);
        }

        private void OnNextPageClick()
        {
            AdvancePageIndex(1);
        }

        private GameObject CreateNavigation()
        {
            var container = new GameObject("PageStackNavigation");

            // TODO(orendain): Remove reliance on this label being first (for _statusText).
            var pageStatus = _uiHelper.CreateText(string.Empty, _uiHelper.DemeoResource.ColorBrown, UiHelper.DefaultButtonFontSize);
            pageStatus.transform.SetParent(container.transform, worldPositionStays: false);

            var prevButton = _uiHelper.CreateButton(OnPreviousPageClick);
            prevButton.transform.SetParent(container.transform, worldPositionStays: false);
            prevButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            prevButton.transform.localPosition = new Vector3(-2.5f, 0, UiHelper.DefaultButtonZShift);

            var prevButtonText = _uiHelper.CreateButtonText("<");
            prevButtonText.transform.SetParent(container.transform, worldPositionStays: false);
            prevButtonText.transform.localPosition = new Vector3(-2.5f, 0, UiHelper.DefaultButtonZShift + UiHelper.DefaultTextZShift);

            var nextButton = _uiHelper.CreateButton(OnNextPageClick);
            nextButton.transform.SetParent(container.transform, worldPositionStays: false);
            nextButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            nextButton.transform.localPosition = new Vector3(2.5f, 0, UiHelper.DefaultButtonZShift);

            var nextButtonText = _uiHelper.CreateButtonText(">");
            nextButtonText.transform.SetParent(container.transform, worldPositionStays: false);
            nextButtonText.transform.localPosition = new Vector3(2.5f, 0, UiHelper.DefaultButtonZShift + UiHelper.DefaultTextZShift);

            return container;
        }

        private void AdvancePageIndex(int advancement)
        {
            _currentPageIndex += advancement;
            _currentPageIndex = Mod(_currentPageIndex, _pages.Count);
            UpdatePageStatus();
            UpdatePageVisibility();
        }

        private void UpdatePageStatus()
        {
            _statusText.text = $"{_currentPageIndex + 1}/{_pages.Count}";
        }

        private void UpdatePageVisibility()
        {
            if (_pages.Count == 0)
            {
                return;
            }

            _pages.ForEach(p => p.SetActive(false));
            _pages[_currentPageIndex].SetActive(true);
        }

        private static int Mod(int x, int m)
        {
            return ((x % m) + m) % m;
        }
    }
}
