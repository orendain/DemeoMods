﻿namespace HouseRules.Configuration.UI
{
    using System.Collections;
    using Common.UI;
    using UnityEngine;

    internal class RulesetSelectionUI : MonoBehaviour
    {
        private UiHelper _uiHelper;
        private GameObject _background;
        private RulesetSelectionPanel _rulesetSelectionPanel;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!UiHelper.IsReady())
            {
                ConfigurationMod.Logger.Msg("UI helper not yet ready. Trying again...");
                yield return new WaitForSecondsRealtime(1);
            }

            ConfigurationMod.Logger.Msg("UI helper ready. Proceeding with initialization.");

            _uiHelper = UiHelper.Instance();
            _rulesetSelectionPanel = RulesetSelectionPanel.NewInstance(HR.Rulebook, _uiHelper);
            Initialize();

            ConfigurationMod.Logger.Msg("Initialization complete.");
        }

        private void Initialize()
        {
            this.transform.SetParent(_uiHelper.DemeoResource.LobbyAnchor.transform, worldPositionStays: true);
            this.transform.position = new Vector3(32, 29.75f, -11);
            this.transform.rotation = Quaternion.Euler(0, 70, 0);

            _background = new GameObject("RulesetSelectionUIBackground");
            _background.AddComponent<MeshFilter>().mesh = _uiHelper.DemeoResource.MenuBoxMesh;
            _background.AddComponent<MeshRenderer>().material = _uiHelper.DemeoResource.MenuBoxMaterial;

            _background.transform.SetParent(this.transform, worldPositionStays: false);
            _background.transform.localPosition = new Vector3(0, -3.25f, 0);
            _background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            _background.transform.localScale = new Vector3(2.5f, 1, 2.5f);

            var menuTitle = _uiHelper.CreateMenuHeaderText("HouseRules");
            menuTitle.transform.SetParent(this.transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 2.725f, 0);

            var selectionPanel = _rulesetSelectionPanel.GameObject;
            selectionPanel.transform.SetParent(this.transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector3(-0, -1f, 0);

            // TODO(orendain): Fix so that ray interacts with entire object.
            this.gameObject.AddComponent<BoxCollider>();
        }
    }
}
