namespace HouseRules.Configuration.UI
{
    using System.Collections;
    using System.Linq;
    using Common.UI;
    using Common.UI.Element;
    using UnityEngine;

    internal class HouseRulesUiHangouts : MonoBehaviour
    {
        private VrResourceTable _resourceTable;
        private IElementCreator _elementCreator;
        private RulesetSelectionPanelVr _rulesetPanel;
        private Transform _anchor;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!HangoutsElementCreator.IsReady()
                   || !Resources.FindObjectsOfTypeAll<GameObject>().Any(x => x.name == "GroupLaunchTable"))
            {
                ConfigurationMod.Logger.Msg("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            ConfigurationMod.Logger.Msg("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = HangoutsElementCreator.Instance();
            _rulesetPanel = RulesetSelectionPanelVr.NewInstance(HR.Rulebook, _elementCreator);
            _anchor = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "GroupLaunchTable").transform;

            Initialize();
            ConfigurationMod.Logger.Msg("Initialization complete.");
        }

        private void Initialize()
        {
            transform.SetParent(_anchor, worldPositionStays: true);
            transform.position = new Vector3(0.88f, 2.1f, -3.8f);
            transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);
            gameObject.AddComponent<FaceLocalPlayer>();

            var background = new GameObject("Background");
            background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            background.transform.SetParent(transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, 0, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            background.transform.localScale = new Vector3(3.75f, 1, 2.5f);

            var headerText = _elementCreator.CreateMenuHeaderText("HouseRules");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            headerText.transform.localPosition = new Vector3(0, 5.95f, VrElementCreator.TextZShift);

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
