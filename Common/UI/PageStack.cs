namespace Common.UI
{
    using System.Collections.Generic;
    using Common.UI.Element;
    using TMPro;
    using UnityEngine;

    public class PageStack
    {
        private readonly IElementCreator _elementCreator;
        private readonly List<GameObject> _pages;
        private readonly TMP_Text _statusText;
        private int _currentPageIndex;

        internal PageStackNavigation Navigation { get; }

        public static PageStack NewInstance(IElementCreator elementCreator)
        {
            return new PageStack(elementCreator);
        }

        private PageStack(IElementCreator elementCreator)
        {
            _elementCreator = elementCreator;
            _pages = new List<GameObject>();
            _currentPageIndex = 0;
            Navigation = CreateNavigation();

            _statusText = Navigation.PageStatus.GetComponent<TMP_Text>();
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

        private void OnPreviousPageClick()
        {
            AdvancePageIndex(-1);
        }

        private void OnNextPageClick()
        {
            AdvancePageIndex(1);
        }

        private PageStackNavigation CreateNavigation()
        {
            // TODO(orendain): Remove reliance on this label being first (for _statusText).
            var navigation = new PageStackNavigation();
            navigation.PageStatus = _elementCreator.CreateText(string.Empty, VrResourceTable.ColorBrown, _elementCreator.DefaultButtonFontSize());;
            navigation.PreviousButton = _elementCreator.CreateButton(OnPreviousPageClick);
            navigation.PreviousButtonText = _elementCreator.CreateButtonText("<");
            navigation.NextButton = _elementCreator.CreateButton(OnNextPageClick);
            navigation.NextButtonText = _elementCreator.CreateButtonText(">");
            return navigation;
        }

        private void AdvancePageIndex(int advancement)
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
            _statusText.text = $"{_currentPageIndex + 1}/{_pages.Count}";
        }

        private static int Mod(int x, int m)
        {
            return ((x % m) + m) % m;
        }

        internal class PageStackNavigation
        {
            internal GameObject PageStatus;
            internal GameObject PreviousButton;
            internal GameObject PreviousButtonText;
            internal GameObject NextButton;
            internal GameObject NextButtonText;
        }
    }
}
