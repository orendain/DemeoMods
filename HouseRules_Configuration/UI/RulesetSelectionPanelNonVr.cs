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
    using UnityEngine.UI;

    internal class RulesetSelectionPanelNonVr
    {
        private const int MaxRulesetsPerPage = 7;

        private readonly Rulebook _rulebook;
        private readonly IElementCreator _elementCreator;
        private readonly PageStack _pageStack;

        private TMP_Text _selectedText;

        internal GameObject Panel { get; }

        internal static RulesetSelectionPanelNonVr NewInstance(Rulebook rulebook, IElementCreator elementCreator)
        {
            return new RulesetSelectionPanelNonVr(
                rulebook,
                elementCreator,
                new GameObject("RulesetSelectionPanel"),
                PageStack.NewInstance());
        }

        private RulesetSelectionPanelNonVr(
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

            _pageStack.Navigation.PositionForNonVr();
            var navigation = _pageStack.Navigation.Panel;
            navigation.transform.SetParent(Panel.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector3(0, -610f, 0);
        }

        private GameObject CreateHeader()
        {
            var headerContainer = new GameObject("Header");

            var infoText =
                _elementCreator.CreateNormalText(
                    "Select a ruleset for your next private multiplayer game or skirmish.");
            var rectTransform = (RectTransform)infoText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(500, 100);
            rectTransform.localPosition = new Vector3(0, 0);

            var selectedText = _elementCreator.CreateNormalText("Selected ruleset: ");
            rectTransform = (RectTransform)selectedText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(500, 100);
            rectTransform.localPosition = new Vector3(0, -75f);

            _selectedText = selectedText.GetComponent<TMP_Text>();
            UpdateSelectedText();

            return headerContainer;
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
            container.transform.localPosition = new Vector3(0, -125f, 0);

            for (var i = 0; i < rulesets.Count; i++)
            {
                var yOffset = i * -70f;
                var rulesetRow = CreateRulesetRow(rulesets.ElementAt(i));
                rulesetRow.transform.SetParent(container.transform, worldPositionStays: false);
                rulesetRow.transform.localPosition = new Vector3(0, yOffset, 0);
            }

            return container;
        }

        private GameObject CreateRulesetRow(Ruleset ruleset)
        {
            var container = new GameObject(ruleset.Name);

            var button = _elementCreator.CreateButton(SelectRulesetAction(ruleset.Name));
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(2.2f, 0.75f);
            button.transform.localPosition = new Vector3(-225f, 0);

            var buttonText =
                _elementCreator.CreateText(ruleset.Name, Color.white, NonVrElementCreator.DefaultLabelFontSize);
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localPosition = new Vector3(-225f, 0);
            buttonText.GetComponent<Graphic>().raycastTarget = false;

            var description = _elementCreator.CreateNormalText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(450, 50);
            rectTransform.localPosition = new Vector3(-485f, -25f);

            return container;
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
