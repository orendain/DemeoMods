namespace Common.UI
{
    using Common.UI.Element;
    using TMPro;
    using UnityEngine;

    public class PageStackNavigation
    {
        private readonly PageStack _pageStack;
        private readonly IElementCreator _elementCreator;

        private GameObject _pageStatus;
        private GameObject _previousButton;
        private GameObject _previousButtonText;
        private GameObject _nextButton;
        private GameObject _nextButtonText;

        public GameObject Panel { get; }

        public TMP_Text PageStatusText { get; }

        public static PageStackNavigation NewInstance(PageStack pageStack)
        {
            if (Environments.CurrentEnvironment() == Environment.NonVr)
            {
                return new PageStackNavigation(pageStack, NonVrElementCreator.Instance());
            }

            if (Environments.CurrentEnvironment() == Environment.Hangouts)
            {
                return new PageStackNavigation(pageStack, HangoutsElementCreator.Instance());
            }

            return new PageStackNavigation(pageStack, VrElementCreator.Instance());
        }

        private PageStackNavigation(PageStack pageStack, IElementCreator elementCreator)
        {
            _pageStack = pageStack;
            _elementCreator = elementCreator;

            _previousButton = elementCreator.CreateButton(OnPreviousPageClick);
            _previousButtonText = elementCreator.CreateButtonText("<");
            _nextButton = elementCreator.CreateButton(OnNextPageClick);
            _nextButtonText = elementCreator.CreateButtonText(">");

            if (Environments.CurrentEnvironment() == Environment.NonVr)
            {
                InitializeForNonVr();
            }
            else
            {
                InitializeForVr();
            }

            Panel = new GameObject("PageStackNavigation");
            _pageStatus.transform.SetParent(Panel.transform, worldPositionStays: false);
            _previousButton.transform.SetParent(Panel.transform, worldPositionStays: false);
            _previousButtonText.transform.SetParent(Panel.transform, worldPositionStays: false);
            _nextButton.transform.SetParent(Panel.transform, worldPositionStays: false);
            _nextButtonText.transform.SetParent(Panel.transform, worldPositionStays: false);

            PageStatusText = _pageStatus.GetComponent<TMP_Text>();
        }

        private void InitializeForNonVr()
        {
            _pageStatus = _elementCreator.CreateText(
                string.Empty,
                NonVrElementCreator.ColorBrown,
                NonVrElementCreator.ButtonFontSize);
        }

        private void InitializeForVr()
        {
            _pageStatus = _elementCreator.CreateText(
                string.Empty,
                VrElementCreator.ColorBrown,
                VrElementCreator.ButtonFontSize);
        }

        public void PositionForNonVr()
        {
            _previousButton.transform.localScale = new Vector2(0.8f, 0.6f);
            _previousButton.transform.localPosition = new Vector2(-80f, 0);

            _previousButtonText.transform.localPosition = new Vector2(-80f, 0);

            _nextButton.transform.localScale = new Vector2(0.8f, 0.6f);
            _nextButton.transform.localPosition = new Vector2(80f, 0);

            _nextButtonText.transform.localPosition = new Vector2(80f, 0);
        }

        public void PositionForVr()
        {
            _previousButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            _previousButton.transform.localPosition = new Vector3(-2.5f, 0, VrElementCreator.ButtonZShift);

            _previousButtonText.transform.localPosition = new Vector3(
                -2.5f,
                0,
                VrElementCreator.ButtonZShift + VrElementCreator.TextZShift);

            _nextButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            _nextButton.transform.localPosition = new Vector3(2.5f, 0, VrElementCreator.ButtonZShift);

            _nextButtonText.transform.localPosition = new Vector3(
                2.5f,
                0,
                VrElementCreator.ButtonZShift + VrElementCreator.TextZShift);
        }

        private void OnPreviousPageClick()
        {
            _pageStack.AdvancePageIndex(-1);
        }

        private void OnNextPageClick()
        {
            _pageStack.AdvancePageIndex(1);
        }
    }
}
