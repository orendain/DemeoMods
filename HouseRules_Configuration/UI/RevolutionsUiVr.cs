namespace HouseRules.Configuration.UI
{
    using System.Collections;
    using System.Linq;
    using System.Text;
    using Common.UI;
    using Common.UI.Element;
    using UnityEngine;

    internal class RevolutionsUiVr : MonoBehaviour
    {
        private VrResourceTable _resourceTable;
        private IElementCreator _elementCreator;
        private Transform _anchor;

        private void Start()
        {
            // ToFindObjects();
            StartCoroutine(WaitAndInitialize());
        }

        /*public static void ToFindObjects()
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                {
                    ConfigurationMod.Logger.Msg($"{go.name}");
                }
            }
        }*/

        private IEnumerator WaitAndInitialize()
        {
            ConfigurationMod.Logger.Msg("RevolutionsUiVr called...");
            while (!VrElementCreator.IsReady()
                   || Resources
                       .FindObjectsOfTypeAll<GameObject>()
                       .Count(x => x.name == "~LeanTween") < 1)
            {
                ConfigurationMod.Logger.Msg("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            ConfigurationMod.Logger.Msg("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = VrElementCreator.Instance();
            _anchor = Resources
                .FindObjectsOfTypeAll<GameObject>()
                .First(x => x.name == "~LeanTween").transform;

            Initialize();
            ConfigurationMod.Logger.Msg("Initialization complete.");
        }

        private void Initialize()
        {
            transform.SetParent(_anchor, worldPositionStays: true);
            transform.position = new Vector3(35.6f, 26.4f, -32.2f);

            gameObject.AddComponent<FaceLocalPlayer>();

            var background = new GameObject("Background");
            background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            background.transform.SetParent(transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, 0, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            background.transform.localScale = new Vector3(3.75f, 1, 2.5f);

            var headerText = _elementCreator.CreateMenuHeaderText("Demeo Revolutions");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            headerText.transform.localPosition = new Vector3(0, 5.95f, VrElementCreator.TextZShift);

            Color lightblue = new Color(0f, 0.75f, 1f);
            Color lightgreen = new Color(0f, 1f, 0.5f);
            Color orange = new Color(1f, 0.499f, 0f);
            Color pink = new Color(0.984f, 0.3765f, 0.498f);
            Color gold = new Color(1f, 1f, 0.6f);
            Color mustard = new Color(0.73f, 0.55f, 0.07f);
            var sb = new StringBuilder();
            sb.AppendLine(ColorizeString("<u>Ruleset details will be listed here</u>", Color.yellow));
            sb.Append(ColorizeString("Days working on this so far: ", pink));
            sb.AppendLine(ColorizeString("1", Color.green));
            var detailsPanel = _elementCreator.CreateNewText(sb.ToString());
            detailsPanel.transform.SetParent(transform, worldPositionStays: false);
            detailsPanel.transform.localPosition = new Vector3(0, 2.5f, VrElementCreator.TextZShift);

            var versionText = _elementCreator.CreateNormalText($"v{BuildVersion.Version}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            versionText.transform.localPosition = new Vector3(-7, -15.85f, VrElementCreator.TextZShift);

            /*if (ConfigurationMod.IsUpdateAvailable)
            {
                var updateText = _elementCreator.CreateNormalText("NEW UPDATE AVAILABLE!");
                updateText.transform.SetParent(transform, worldPositionStays: false);
                updateText.transform.localPosition = new Vector3(4.8f, -15.85f, VrElementCreator.TextZShift);
            }*/

            // TODO(orendain): Fix so that ray interacts with entire object.
            gameObject.AddComponent<BoxCollider>();
        }

        private static string ColorizeString(string text, Color color)
        {
            return string.Concat(new string[]
            {
        "<color=#",
        ColorUtility.ToHtmlStringRGB(color),
        ">",
        text,
        "</color>",
            });
        }
    }
}
