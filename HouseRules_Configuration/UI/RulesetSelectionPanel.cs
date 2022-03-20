namespace HouseRules.Configuration.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.UI;
    using HouseRules.Types;
    using TMPro;
    using UnityEngine;

    internal class RulesetSelectionPanel
    {
        private const int MaxRulesetsPerPage = 7;

        private readonly Rulebook _rulebook;
        private readonly UiHelper _uiHelper;
        private readonly PageStack _pageStack;

        private TextMeshPro _selectedText;

        internal GameObject Panel { get; }

        internal static RulesetSelectionPanel NewInstance(Rulebook rulebook, UiHelper uiHelper)
        {
            return new RulesetSelectionPanel(
                rulebook,
                uiHelper,
                new GameObject("RulesetSelectionPanel"),
                PageStack.NewInstance(uiHelper));
        }

        private RulesetSelectionPanel(Rulebook rulebook, UiHelper uiHelper, GameObject panel, PageStack pageStack)
        {
            this._rulebook = rulebook;
            this._uiHelper = uiHelper;
            this._pageStack = pageStack;
            this.Panel = panel;

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

            var pageNavigation = _pageStack.NavigationPanel;
            pageNavigation.transform.SetParent(Panel.transform, worldPositionStays: false);
            pageNavigation.transform.localPosition = new Vector3(0, -17f, 0);
        }

        private GameObject CreateHeader()
        {
            var headerContainer = new GameObject("Header");

            var infoText = _uiHelper.CreateLabelText("Select a ruleset for your next private multiplayer game or skirmish.");
            var rectTransform = (RectTransform)infoText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(10, 2);
            rectTransform.localPosition = new Vector3(0, 0, UiHelper.DefaultTextZShift);

            var selectedText = _uiHelper.CreateLabelText("Selected ruleset: ");
            rectTransform = (RectTransform)selectedText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(10, 2);
            rectTransform.localPosition = new Vector3(0, -1.5f, UiHelper.DefaultTextZShift);

            _selectedText = selectedText.GetComponentInChildren<TextMeshPro>();
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

            var button = _uiHelper.CreateButton(SelectRulesetAction(ruleset.Name));
            button.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(1f, 0.6f, 1f);
            button.transform.localPosition = new Vector3(-4.5f, 0, UiHelper.DefaultButtonZShift);

            var buttonText = _uiHelper.CreateText(ruleset.Name, Color.white, UiHelper.DefaultLabelFontSize);
            buttonText.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            buttonText.transform.localPosition =
                new Vector3(-4.5f, 0, UiHelper.DefaultButtonZShift + UiHelper.DefaultTextZShift);

            var description = _uiHelper.CreateLabelText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(9, 1);
            rectTransform.localPosition = new Vector3(-9.7f, -0.5f, UiHelper.DefaultTextZShift);

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
