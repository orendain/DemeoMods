﻿namespace HouseRules.Configuration.UI
{
    using System.Collections;
    using System.Linq;
    using Common.UI;
    using Common.UI.Element;
    using UnityEngine;

    internal class HouseRulesUiVr : MonoBehaviour
    {
        private VrResourceTable _resourceTable;
        private IElementCreator _elementCreator;
        private GameObject _background;
        private RulesetSelectionPanelVr _rulesetPanel;
        private Transform _anchor;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!VrElementCreator.IsReady()
                   || Resources.FindObjectsOfTypeAll<charactersoundlistener>()
                       .Count(x => x.name == "MenuBox_BindPose") < 2)
            {
                ConfigurationMod.Logger.Msg("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            ConfigurationMod.Logger.Msg("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = VrElementCreator.Instance();
            _rulesetPanel = RulesetSelectionPanelVr.NewInstance(HR.Rulebook, _elementCreator);
            _anchor = Resources.FindObjectsOfTypeAll<charactersoundlistener>()
                .First(x => x.name == "MenuBox_BindPose").transform;

            Initialize();
            ConfigurationMod.Logger.Msg("Initialization complete.");
        }

        private void Initialize()
        {
            transform.SetParent(_anchor, worldPositionStays: true);
            transform.position = new Vector3(32.6f, 26.4f, -12.8f);
            transform.rotation = Quaternion.Euler(0, 70, 0);

            _background = new GameObject("Background");
            _background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            _background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            _background.transform.SetParent(transform, worldPositionStays: false);
            _background.transform.localPosition = new Vector3(0, 0, 0);
            _background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            _background.transform.localScale = new Vector3(3.75f, 1, 2.5f);

            var menuTitle = _elementCreator.CreateMenuHeaderText("HouseRules");
            menuTitle.transform.SetParent(transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 5.95f, VrElementCreator.TextZShift);

            var selectionPanel = _rulesetPanel.Panel;
            selectionPanel.transform.SetParent(transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector3(0, 2.5f, 0);

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-7, -15.85f, VrElementCreator.TextZShift);

            if (ConfigurationMod.IsUpdateAvailable)
            {
                var updateText = _elementCreator.CreateNormalText("NEW UPDATE AVAILABLE!");
                updateText.transform.SetParent(transform, worldPositionStays: false);
                updateText.transform.localPosition = new Vector3(4.8f, -15.85f, VrElementCreator.TextZShift);
            }

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();
        }
    }
}
