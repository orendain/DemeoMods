namespace HouseRules.Configuration.UI
{
    using System;
    using System.Linq;
    using Common.UI;
    using HouseRules.Types;
    using UnityEngine;

    internal class RulesetSelectionPanel
    {
        private readonly UiHelper _uiHelper;

        private readonly Rulebook _rulebook;

        internal GameObject GameObject { get; }

        internal static RulesetSelectionPanel NewInstance(Rulebook rulebook, UiHelper uiHelper)
        {
            return new RulesetSelectionPanel(rulebook, uiHelper, new GameObject("RulesetSelectionPanel"));
        }

        private RulesetSelectionPanel(Rulebook rulebook, UiHelper uiHelper, GameObject panel)
        {
            this._rulebook = rulebook;
            this._uiHelper = uiHelper;
            this.GameObject = panel;

            Render();
        }

        private void Render()
        {
            var headerContainer = CreateHeader();
            headerContainer.transform.SetParent(GameObject.transform, worldPositionStays: false);

            var rulesetsContainer = new GameObject("Rulesets");
            rulesetsContainer.transform.SetParent(GameObject.transform, worldPositionStays: false);
            rulesetsContainer.transform.localPosition = new Vector3(0, -2f, 0);

            var rulesetRow = CreateRulesetRow(Ruleset.None);
            rulesetRow.transform.SetParent(rulesetsContainer.transform, worldPositionStays: false);
            rulesetRow.transform.localPosition = new Vector3(0, 0, 0);

            for (var i = 0; i < _rulebook.Rulesets.Count; i++)
            {
                var yOffset = (i + 1) * -3f;
                rulesetRow = CreateRulesetRow(_rulebook.Rulesets.ElementAt(i));
                rulesetRow.transform.SetParent(rulesetsContainer.transform, worldPositionStays: false);
                rulesetRow.transform.localPosition = new Vector3(0, yOffset, 0);
            }
        }

        private GameObject CreateHeader()
        {
            var headerContainer = new GameObject("RulesetSelectionHeader");

            var infoText = _uiHelper.CreateLabelText($"Select a ruleset for your next\nprivate multiplayer game or skirmish.");
            infoText.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            infoText.transform.localPosition = new Vector3(0f, 1f, 0);

            return headerContainer;
        }

        private GameObject CreateRulesetRow(Ruleset ruleset)
        {
            var roomRowContainer = new GameObject(ruleset.Name);

            var button = _uiHelper.CreateButton(SelectRulesetAction(ruleset.Name));
            button.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(2f, 2f, 1);
            button.transform.localPosition = new Vector3(0, 0, 0);

            var buttonTextString = $"{ruleset.Name}\n{ruleset.Description}";

            var buttonText = _uiHelper.CreateText(buttonTextString, Color.white, UiHelper.DefaultLabelFontSize);
            buttonText.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            buttonText.transform.localPosition = new Vector3(0, 0, 0);

            return roomRowContainer;
        }

        private static Action SelectRulesetAction(string rulesetName)
        {
            return () => HR.SelectRuleset(rulesetName);
        }
    }
}
