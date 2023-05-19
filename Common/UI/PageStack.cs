namespace Common.UI
{
    using System.Collections.Generic;
    using Common.UI.Element;
    using UnityEngine;

    public class PageStack
    {
        private readonly List<GameObject> _pages;
        private int _currentPageIndex;

        public PageStackNavigation Navigation { get; }

        public static PageStack NewInstance(IElementCreator elementCreator)
        {
            return new PageStack(elementCreator);
        }

        private PageStack(IElementCreator elementCreator)
        {
            _pages = new List<GameObject>();
            _currentPageIndex = 0;
            Navigation = PageStackNavigation.NewInstance(this, elementCreator);

            UpdatePageStatus();
        }

        /// <summary>
        /// Adds the specified page to the stack navigation order.
        /// </summary>
        public void AddPage(GameObject page)
        {
            _pages.Add(page);
            UpdatePageVisibility();
            UpdatePageStatus();
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

        public void AdvancePageIndex(int advancement)
        {
            _currentPageIndex += advancement;
            _currentPageIndex = Mod(_currentPageIndex, _pages.Count);
            UpdatePageStatus();
            UpdatePageVisibility();
        }

        private void UpdatePageVisibility()
        {
            if (_pages.Count < 1)
            {
                return;
            }

            _pages.ForEach(p => p.SetActive(false));
            _pages[_currentPageIndex].SetActive(true);
        }

        private void UpdatePageStatus()
        {
            Navigation.PageStatusText.text = $"{_currentPageIndex + 1}/{_pages.Count}";
        }

        private static int Mod(int x, int m)
        {
            return ((x % m) + m) % m;
        }
    }
}
