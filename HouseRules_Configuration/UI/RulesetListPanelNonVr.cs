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

    internal class RulesetListPanelNonVr
    {
        private const int MaxRulesetsPerPage = 7;

        private readonly Rulebook _rulebook;
        private readonly IElementCreator _elementCreator;
        private readonly PageStack _pageStack;

        private TMP_Text _selectedText;

        internal GameObject Panel { get; }

        internal static RulesetListPanelNonVr NewInstance(Rulebook rulebook, IElementCreator elementCreator)
        {
            return new RulesetListPanelNonVr(
                rulebook,
                elementCreator,
                PageStack.NewInstance(elementCreator));
        }

        private RulesetListPanelNonVr(
            Rulebook rulebook,
            IElementCreator elementCreator,
            PageStack pageStack)
        {
            _rulebook = rulebook;
            _elementCreator = elementCreator;
            _pageStack = pageStack;
            Panel = Panel = new GameObject("RulesetListPanel");

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
            rectTransform.sizeDelta = new Vector2(1000, 100);
            rectTransform.localPosition = new Vector2(0, 20f);

            var selectedText = _elementCreator.CreateNormalText("Selected ruleset: ");
            rectTransform = (RectTransform)selectedText.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(1000, 100);
            rectTransform.localPosition = new Vector2(0, -45f);

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
            container.transform.localPosition = new Vector2(0, -115f);

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
            button.transform.localScale = new Vector2(3f, 0.95f);
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localPosition = new Vector2(-325f, 0);

            var buttonText =
                _elementCreator.CreateText(ruleset.Name, Color.white, NonVrElementCreator.ButtonFontSize);
            buttonText.GetComponent<Graphic>().raycastTarget = false;
            var buttonTextTransform = (RectTransform)buttonText.transform;
            buttonTextTransform.SetParent(container.transform, worldPositionStays: false);
            buttonTextTransform.sizeDelta = new Vector2(300, 50);
            buttonTextTransform.localPosition = new Vector2(-325f, 1);

            var description = _elementCreator.CreateNormalText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(700, 50);
            rectTransform.localPosition = new Vector2(-825f, -25f);

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
            var setName = HR.SelectedRuleset.Name;
            if (setName.Equals("None"))
            {
                setName = "<color=#B31106FF><b>None</b></color>";
            }
            else if (setName.Contains("Demeo Revolutions"))
            {
                if (setName.Contains("(EASY"))
                {
                    setName = "<color=#057004><b>Demeo Revolutions</b></color> <color=#1D21E0><b>(EASY)</b></color>";
                }
                else if (setName.Contains("(HARD"))
                {
                    setName = "<color=#057004><b>Demeo Revolutions</b></color> <color=#5611A2><b>(HARD)</b></color>";
                }
                else if (setName.Contains("(LEGENDARY"))
                {
                    setName = "<color=#057004><b>Demeo Revolutions</b></color> <color=#9F11A2><b>(LEGENDARY)</b></color>";
                }
                else if (setName.Contains("(Small"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#A2115D><b>(Small PROGRESSIVE)</b></color>";
                }
                else if (setName.Contains("PROGRESSIVE"))
                {
                    setName = "<color=#057004><b>Demeo Revolutions</b></color> <color=#A2115D><b>(PROGRESSIVE)</b></color>";
                }
                else
                {
                    setName = "<color=#057004><b>Demeo Revolutions</b></color>";
                }
            }

            _selectedText.text = $"Selected ruleset: {setName}";
        }
    }
}
