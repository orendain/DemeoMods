namespace HouseRules.Configuration.UI
{
    using System;
    using System.Linq;
    using Common.UI;
    using DataKeys;
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
            rulesetsContainer.transform.localPosition = new Vector3(0, -1.5f, 0);

            var rulesetRow = CreateRulesetRow(Ruleset.None);
            rulesetRow.transform.SetParent(rulesetsContainer.transform, worldPositionStays: false);
            rulesetRow.transform.localPosition = new Vector3(0, 0, 0);

            for (var i = 0; i < _rulebook.Rulesets.Count; i++)
            {
                var yOffset = (i + 1) * -1.75f;
                rulesetRow = CreateRulesetRow(_rulebook.Rulesets.ElementAt(i));
                rulesetRow.transform.SetParent(rulesetsContainer.transform, worldPositionStays: false);
                rulesetRow.transform.localPosition = new Vector3(0, yOffset, 0);
            }
        }

        private GameObject CreateHeader()
        {
            var headerContainer = new GameObject("RulesetsHeader");

            var infoText = _uiHelper.CreateLabelText("Select a ruleset for your next private multiplayer game or skirmish.");
            var rectTransform = (RectTransform)infoText.transform;
            rectTransform.SetParent(headerContainer.transform, worldPositionStays: false);
            rectTransform.sizeDelta = new Vector2(10, 2);
            rectTransform.localPosition = new Vector3(0f, 1f, UiHelper.DefaultTextZShift);

            return headerContainer;
        }

        private GameObject CreateRulesetRow(Ruleset ruleset)
        {
            var roomRowContainer = new GameObject(ruleset.Name);

            var rawButton = _uiHelper.CreateButton(SelectRulesetAction(ruleset.Name));
            var button = UiHelper.WrapObject(rawButton);
            button.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            button.transform.localScale = new Vector3(1.5f, 1f, 1);
            button.transform.localPosition = new Vector3(-4.5f, 0, 0);

            var buttonText = _uiHelper.CreateText(ruleset.Name, Color.white, UiHelper.DefaultLabelFontSize);
            buttonText.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            buttonText.transform.localPosition = new Vector3(-4.5f, 0, UiHelper.DefaultTextZShift);

            var description = _uiHelper.CreateLabelText(ruleset.Description);
            var rectTransform = (RectTransform)description.transform;
            rectTransform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            rectTransform.pivot = Vector2.left;
            rectTransform.sizeDelta = new Vector2(9, 1);
            rectTransform.localPosition = new Vector3(-9.7f, -0.5f, UiHelper.DefaultTextZShift);

            return roomRowContainer;
        }

        private static Action SelectRulesetAction(string rulesetName)
        {
            return () => HR.SelectRuleset(rulesetName);
        }
    }
}
