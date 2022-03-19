namespace Common.UI
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    internal class PageStack
    {
        private readonly GameObject _panel;
        private readonly GameObject _navigation;

        private readonly UiHelper _uiHelper;

        private readonly List<GameObject> _pages;
        private int _currentPageIndex;

        public static PageStack NewInstance(UiHelper uiHelper)
        {
            return new PageStack(uiHelper);
        }

        private PageStack(UiHelper uiHelper)
        {
            _panel = new GameObject("PageStack");
            _uiHelper = uiHelper;

            _pages = new List<GameObject>();
            _currentPageIndex = 0;

            _navigation = CreateNavigation();
            UpdatePageStatus();
        }

        /// <summary>
        /// Adds the specified page to the stack.
        /// </summary>
        /// <remarks>
        /// Any relative positioning of the page is preserved, and its rotation is modified.
        /// </remarks>
        public void AddPage(GameObject page)
        {
            _pages.Add(page);
            page.transform.SetParent(_panel.transform, worldPositionStays: false);
            page.transform.localRotation = Quaternion.Euler(0, 0, 0);
            page.SetActive(false);
        }

        private void OnPreviousPageClick()
        {
            this._currentPageIndex = --this._currentPageIndex % _pages.Count;
            UpdatePageVisibility();
        }

        private void OnNextPageClick()
        {
            this._currentPageIndex = ++this._currentPageIndex % _pages.Count;
            UpdatePageVisibility();
        }

        private void UpdatePageVisibility()
        {
            _pages.ForEach(p => p.SetActive(false));
            _pages[_currentPageIndex].SetActive(true);
        }

        private GameObject CreateNavigation() {

            var container = new GameObject("PageStackNavigation");

            var prevButton = _uiHelper.CreateButton(OnPreviousPageClick);
            prevButton.transform.SetParent(container.transform, worldPositionStays: false);
            prevButton.transform.localPosition = new Vector3(0, 0, 0);

            var nextButton = _uiHelper.CreateButton(OnNextPageClick);
            nextButton.transform.SetParent(container.transform, worldPositionStays: false);
            nextButton.transform.localPosition = new Vector3(0, 0, 0);

            var pageStatus = _uiHelper.CreateLabelText(string.Empty);
            pageStatus.transform.SetParent(container.transform, worldPositionStays: false);
            pageStatus.transform.localPosition = new Vector3(0, 0, 0);

            return container;
        }

        private void UpdatePageStatus()
        {
            var statusText = _navigation.GetComponentInChildren<TextMeshPro>();
            statusText.text = $"{_currentPageIndex}/{_pages.Count}";
        }
    }
}
