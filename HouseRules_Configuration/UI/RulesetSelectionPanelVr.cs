namespace HouseRules.Configuration.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.UI;
    using Common.UI.Element;
    using HouseRules.Types;
    using TMPro;
    using UnityEngine;

    internal class RulesetSelectionPanelVr
    {
        private const int MaxRulesetsPerPage = 7;

        private readonly Rulebook _rulebook;
        private readonly IElementCreator _elementCreator;
        private readonly PageStack _pageStack;

        private TMP_Text _selectedText;

        internal GameObject Panel { get; }

        internal static RulesetSelectionPanelVr NewInstance(Rulebook rulebook, IElementCreator elementCreator)
        {
            return new RulesetSelectionPanelVr(
                rulebook,
                elementCreator,
                new GameObject("RulesetSelectionPanel"),
                PageStack.NewInstance(elementCreator));
        }

        private RulesetSelectionPanelVr(
            Rulebook rulebook,
            IElementCreator elementCreator,
            GameObject panel,
            PageStack pageStack)
        {
            _rulebook = rulebook;
            _elementCreator = elementCreator;
            _pageStack = pageStack;
            Panel = panel;

            Render();
        }

        private void Render()
        {
            var header = CreateHeader();
            header.transform.SetParent(Panel.transform, worldPositionStays: false);
            header.transform.localPosition = new Vector3(0, 1f, 0);

            var rulesetPartitions = PartitionRulesets();
            var rulesetPages = rulesetPartitions.Select(CreateRulesetPage).ToList();
            rulesetPages.ForEach(_pageStack.AddPage);

            var pageNavigation = CreateNavigation();
            pageNavigation.transform.SetParent(Panel.transform, worldPositionStays: false);
            pageNavigation.transform.localPosition = new Vector3(0, -17f, 0);
        }

        private GameObject CreateHeader()
        {
            var headerContainer = new GameObject("Header");

            var infoText =
                _elementCreator.CreateNormalText(
                    "Select a ruleset for your next private multiplayer game or skirmish.");
            var rectTransform = (RectTransform)infoText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(10, 2);
            rectTransform.localPosition = new Vector3(0, 0, VrElementCreator.DefaultTextZShift);

            var selectedText = _elementCreator.CreateNormalText("Selected ruleset: ");
            rectTransform = (RectTransform)selectedText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(10, 2);
            rectTransform.localPosition = new Vector3(0, -1.5f, VrElementCreator.DefaultTextZShift);

            _selectedText = selectedText.GetComponent<TMP_Text>();
            UpdateSelectedText();

            return headerContainer;
        }

        private GameObject CreateNavigation()
        {
            var container = new GameObject("PageStackNavigation");

            _pageStack.Navigation.PageStatus.transform.SetParent(container.transform, worldPositionStays: false);

            _pageStack.Navigation.PreviousButton.transform.SetParent(container.transform, worldPositionStays: false);
            _pageStack.Navigation.PreviousButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            _pageStack.Navigation.PreviousButton.transform.localPosition =
                new Vector3(-2.5f, 0, VrElementCreator.DefaultButtonZShift);

            _pageStack.Navigation.PreviousButtonText.transform.SetParent(
                container.transform,
                worldPositionStays: false);
            _pageStack.Navigation.PreviousButtonText.transform.localPosition = new Vector3(
                -2.5f,
                0,
                VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            _pageStack.Navigation.NextButton.transform.SetParent(container.transform, worldPositionStays: false);
            _pageStack.Navigation.NextButton.transform.localScale = new Vector3(0.25f, 0.8f, 0.8f);
            _pageStack.Navigation.NextButton.transform.localPosition =
                new Vector3(2.5f, 0, VrElementCreator.DefaultButtonZShift);

            _pageStack.Navigation.NextButtonText.transform.SetParent(container.transform, worldPositionStays: false);
            _pageStack.Navigation.NextButtonText.transform.localPosition = new Vector3(
                2.5f,
                0,
                VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            return container;
        }

        private IEnumerable<List<Ruleset>> PartitionRulesets()
        {
            return _rulebook.Rulesets
                .Select((value, index) => new { group = index / MaxRulesetsPerPage, value })
                .GroupBy(pair => pair.group)
                .Select(group => group.Select(g => g.value).ToList());
        }

        private GameObject CreateRulesetPage(List<Ruleset> rulesets)
        {
            var container = new GameObject("Rulesets");
            container.transform.SetParent(Panel.transform, worldPositionStays: false);
            container.transform.localPosition = new Vector3(0, -2.5f, 0);

            for (var i = 0; i < rulesets.Count; i++)
            {
                var yOffset = i * -2f;
                var rulesetRow = CreateRulesetRow(rulesets.ElementAt(i));
                rulesetRow.transform.SetParent(container.transform, worldPositionStays: false);
                rulesetRow.transform.localPosition = new Vector3(0, yOffset, 0);
            }

            return container;
        }

        private GameObject CreateRulesetRow(Ruleset ruleset)
        {
            var roomRowContainer = new GameObject(ruleset.Name);

            var button = _elementCreator.CreateButton(SelectRulesetAction(ruleset.Name));
            button.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(1f, 0.6f, 1f);
            button.transform.localPosition = new Vector3(-4.5f, 0, VrElementCreator.DefaultButtonZShift);

            var buttonText =
                _elementCreator.CreateText(ruleset.Name, Color.white, VrElementCreator.DefaultLabelFontSize);
            buttonText.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            buttonText.transform.localPosition = new Vector3(
                -4.5f,
                0,
                VrElementCreator.DefaultButtonZShift + VrElementCreator.DefaultTextZShift);

            var description = _elementCreator.CreateNormalText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(9, 1);
            rectTransform.localPosition = new Vector3(-9.7f, -0.5f, VrElementCreator.DefaultTextZShift);

            return roomRowContainer;
        }

        private Action SelectRulesetAction(string rulesetName)
        {
            return () =>
            {
                HR.SelectRuleset(rulesetName);
                UpdateSelectedText();
            };
        }

        private void UpdateSelectedText()
        {
            _selectedText.text = $"Selected ruleset: {HR.SelectedRuleset.Name}";
        }
    }
}
