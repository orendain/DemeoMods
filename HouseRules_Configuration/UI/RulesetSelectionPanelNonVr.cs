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
                PageStack.NewInstance());
        }

        private RulesetSelectionPanelNonVr(
            Rulebook rulebook,
            IElementCreator elementCreator,
            PageStack pageStack)
        {
            _rulebook = rulebook;
            _elementCreator = elementCreator;
            _pageStack = pageStack;
            Panel = Panel = new GameObject("RulesetSelectionPanel");

            Render();
        }

        private void Render()
        {
            var header = CreateHeader();
            header.transform.SetParent(Panel.transform, worldPositionStays: false);
            header.transform.localPosition = new Vector2(0, 1f);

            var rulesetPartitions = PartitionRulesets();
            var rulesetPages = rulesetPartitions.Select(CreateRulesetPage).ToList();
            rulesetPages.ForEach(_pageStack.AddPage);

            _pageStack.Navigation.PositionForNonVr();
            var navigation = _pageStack.Navigation.Panel;
            navigation.transform.SetParent(Panel.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector2(0, -610f);
        }

        private GameObject CreateHeader()
        {
            var container = new GameObject("Header");

            var infoText =
                _elementCreator.CreateNormalText(
                    "Select a ruleset for your next private multiplayer game or skirmish.");
            var rectTransform = (RectTransform)infoText.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(500, 100);

            var selectedText = _elementCreator.CreateNormalText("Selected ruleset: ");
            rectTransform = (RectTransform)selectedText.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(500, 100);
            rectTransform.localPosition = new Vector2(0, -75f);

            _selectedText = selectedText.GetComponent<TMP_Text>();
            UpdateSelectedText();

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
            container.transform.localPosition = new Vector2(0, -125f);

            for (var i = 0; i < rulesets.Count; i++)
            {
                var yOffset = i * -70f;
                var row = CreateRulesetRow(rulesets.ElementAt(i));
                row.transform.SetParent(container.transform, worldPositionStays: false);
                row.transform.localPosition = new Vector2(0, yOffset);
            }

            return container;
        }

        private GameObject CreateRulesetRow(Ruleset ruleset)
        {
            var container = new GameObject(ruleset.Name);

            var button = _elementCreator.CreateButton(SelectRulesetAction(ruleset.Name));
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector2(2.2f, 0.75f);
            button.transform.localPosition = new Vector2(-225f, 0);

            var buttonText =
                _elementCreator.CreateText(ruleset.Name, Color.white, NonVrElementCreator.NormalFontSize);
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localPosition = new Vector2(-225f, 0);
            buttonText.GetComponent<Graphic>().raycastTarget = false;

            var description = _elementCreator.CreateNormalText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(450, 50);
            rectTransform.localPosition = new Vector2(-485f, -25f);

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
