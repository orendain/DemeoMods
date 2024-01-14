namespace HouseRules.Configuration.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.UI;
    using Common.UI.Element;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using TMPro;
    using UnityEngine;

    internal class RulesetListPanelVr
    {
        private const int MaxRulesetsPerPage = 7;

        private readonly Rulebook _rulebook;
        private readonly IElementCreator _elementCreator;
        private readonly PageStack _pageStack;

        private TMP_Text _selectedText;

        internal GameObject Panel { get; }

        internal static RulesetListPanelVr NewInstance(Rulebook rulebook, IElementCreator elementCreator)
        {
            return new RulesetListPanelVr(
                rulebook,
                elementCreator,
                PageStack.NewInstance(elementCreator));
        }

        private RulesetListPanelVr(
            Rulebook rulebook,
            IElementCreator elementCreator,
            PageStack pageStack)
        {
            _rulebook = rulebook;
            _elementCreator = elementCreator;
            _pageStack = pageStack;
            Panel = new GameObject("RulesetListPanel");

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

            _pageStack.Navigation.PositionForVr();
            var navigation = _pageStack.Navigation.Panel;
            navigation.transform.SetParent(Panel.transform, worldPositionStays: false);
            navigation.transform.localPosition = new Vector3(0, -17f, 0);
        }

        private GameObject CreateHeader()
        {
            var container = new GameObject("Header");

            var infoText =
                _elementCreator.CreateNormalText(
                    "Select a ruleset for your next private multiplayer game or skirmish.");
            var rectTransform = (RectTransform)infoText.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(20, 2);
            rectTransform.localPosition = new Vector3(0, 0, VrElementCreator.TextZShift);

            var selectedText = _elementCreator.CreateNormalText("Selected ruleset: ");
            rectTransform = (RectTransform)selectedText.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(20, 2);
            rectTransform.localPosition = new Vector3(0, -1.5f, VrElementCreator.TextZShift);

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
            container.transform.localPosition = new Vector3(0, -2.5f, 0);

            for (var i = 0; i < rulesets.Count; i++)
            {
                var yOffset = i * -2f;
                var row = CreateRulesetRow(rulesets.ElementAt(i));
                row.transform.SetParent(container.transform, worldPositionStays: false);
                row.transform.localPosition = new Vector3(0, yOffset, 0);
            }

            return container;
        }

        private GameObject CreateRulesetRow(Ruleset ruleset)
        {
            var container = new GameObject(ruleset.Name);

            var button = _elementCreator.CreateButton(SelectRulesetAction(ruleset.Name));
            button.transform.SetParent(container.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(1f, 0.9f, 1f);
            button.transform.localPosition = new Vector3(-4.5f, 0, VrElementCreator.ButtonZShift);

            var buttonText =
                _elementCreator.CreateText(ruleset.Name, Color.white, VrElementCreator.ButtonFontSize);
            buttonText.transform.SetParent(container.transform, worldPositionStays: false);
            buttonText.transform.localPosition = new Vector3(
                -4.5f,
                0,
                VrElementCreator.ButtonZShift + VrElementCreator.TextZShift);

            var description = _elementCreator.CreateNormalText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(container.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(9, 1);
            rectTransform.localPosition = new Vector3(-9.7f, -0.5f, VrElementCreator.TextZShift);

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
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#1D21E0><b>(EASY)</b></color>";
                }
                else if (setName.Contains("(HARD"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#5611A2><b>(HARD)</b></color>";
                }
                else if (setName.Contains("(LEGENDARY PROGRESSIVE"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#9F11A2><b>(LEGENDARY PROGRESSIVE)</b></color>";
                }
                else if (setName.Contains("(LEGENDARY"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#9F11A2><b>(LEGENDARY)</b></color>";
                }
                else if (setName.Contains("(SMALL"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#1D21E0><b>(SMALL PROGRESSIVE)</b></color>";
                }
                else if (setName.Contains("PROGRESSIVE"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color> <color=#A2115D><b>(PROGRESSIVE)</b></color>";
                }
                else if (setName.Equals("Demeo Revolutions"))
                {
                    setName = "<color=#01550A><b>Demeo Revolutions</b></color>";
                }
                else
                {
                    string tempName = setName;
                    setName = "<color=#01550A><b>" + tempName + "</b></color>";
                }
            }
            else if (setName.Equals("Friendly Competition"))
            {
                setName = "<color=#1D1E00><b>Friendly Competition</b></color>";
            }
            else if (setName.Equals("Darkest Dankest Demeo"))
            {
                setName = "<color=#080800><b>Darkest Dankest Demeo</b></color>";
            }
            else if (setName.Contains("Voice Machine (PROGRESSIVE"))
            {
                setName = "<color=#AAEA04><b>Heroes of the Voice Machine</b></color> <color=#9F11A2><b>(PROGRESSIVE)</b></color>";
            }

            _selectedText.text = $"Selected ruleset: {setName}";
        }
    }
}
