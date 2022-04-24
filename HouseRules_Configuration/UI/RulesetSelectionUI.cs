namespace HouseRules.Configuration.UI
{
    using System;
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
            switch (UiHelper.GetCurrentUiType())
            {
                case UiHelper.UiType.Vr:
                    PositionInVrLobby();
                    break;
                case UiHelper.UiType.Hangouts:
                    PositionInHangouts();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported Demeo UI.");
            }

            _background = new GameObject("Background");
            _background.AddComponent<MeshFilter>().mesh = _uiHelper.DemeoResource.MenuBoxMesh;
            _background.AddComponent<MeshRenderer>().material = _uiHelper.DemeoResource.MenuBoxMaterial;
            _background.transform.SetParent(this.transform, worldPositionStays: false);
            _background.transform.localPosition = new Vector3(0, 0, 0);
            _background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            _background.transform.localScale = new Vector3(3.75f, 1, 2.5f);

            var menuTitle = _uiHelper.CreateMenuHeaderText("HouseRules");
            menuTitle.transform.SetParent(this.transform, worldPositionStays: false);
            menuTitle.transform.localPosition = new Vector3(0, 5.95f, UiHelper.DefaultTextZShift);

            var selectionPanel = _rulesetSelectionPanel.Panel;
            selectionPanel.transform.SetParent(this.transform, worldPositionStays: false);
            selectionPanel.transform.localPosition = new Vector3(0, 2.5f, 0);

            var versionText = _uiHelper.CreateLabelText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(this.transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-7, -15.85f, UiHelper.DefaultTextZShift);

            if (VersionChecker.IsUpdateAvailable())
            {
                var updateText = _uiHelper.CreateLabelText("NEW UPDATE AVAILABLE!");
                updateText.transform.SetParent(this.transform, worldPositionStays: false);
                updateText.transform.localPosition = new Vector3(4.8f, -15.85f, UiHelper.DefaultTextZShift);
            }

            // TODO(orendain): Fix so that ray interacts with entire object.
            this.gameObject.AddComponent<BoxCollider>();
        }

        private void PositionInVrLobby()
        {
            this.transform.SetParent(_uiHelper.DemeoResource.VrLobbyTableAnchor.transform, worldPositionStays: true);
            this.transform.position = new Vector3(32.6f, 26.4f, -12.8f);
            this.transform.rotation = Quaternion.Euler(0, 70, 0);
        }

        private void PositionInHangouts()
        {
            this.transform.SetParent(_uiHelper.DemeoResource.HangoutsTableAnchor.transform, worldPositionStays: true);
            this.transform.position = new Vector3(0.88f, 2.1f, -3.8f);
            this.transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);
            this.gameObject.AddComponent<FaceLocalPlayer>();
        }
    }
}
