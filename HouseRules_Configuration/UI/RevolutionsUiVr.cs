namespace HouseRules.Configuration.UI
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using Common.UI;
    using Common.UI.Element;
    using HouseRules.Types;
    using Revolutions;
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
            // ConfigurationMod.Logger.Msg("RevolutionsUiVr called...");
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
            transform.position = new Vector3(35.6f, 36.4f, -32.2f);

            gameObject.AddComponent<FaceLocalPlayer>();

            var background = new GameObject("Background");
            var numRules = HR.SelectedRuleset.Rules.Count;
            background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            background.transform.SetParent(transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, 0, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            if (numRules < 12)
            {
                background.transform.localScale = new Vector3(4.75f, 1, 1.5f);
            }
            else if (numRules < 24)
            {
                background.transform.localScale = new Vector3(4.75f, 1, 2.5f);
            }
            else
            {
                background.transform.localScale = new Vector3(4.75f, 1, 4.5f);
            }

            var headerText = _elementCreator.CreateMenuHeaderText("HouseRules");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            if (numRules < 12)
            {
                headerText.transform.localPosition = new Vector3(0, 3.6f, VrElementCreator.TextZShift);
            }
            else if (numRules < 24)
            {
                headerText.transform.localPosition = new Vector3(0, 6f, VrElementCreator.TextZShift);
            }
            else
            {
                headerText.transform.localPosition = new Vector3(0, 10.5f, VrElementCreator.TextZShift);
            }

            Color indigo = new Color(0.294f, 0f, 0.51f);
            Color lightgreen = new Color(0f, 1f, 0.5f);
            Color orange = new Color(1f, 0.499f, 0f);
            Color pink = new Color(0.984f, 0.3765f, 0.498f);
            Color gold = new Color(1f, 1f, 0.6f);
            Color mustard = new Color(0.73f, 0.55f, 0.07f);
            Color beige = new Color(0.878f, 0.752f, 0.384f, 1);
            Color brown = new Color(0.0392f, 0.0157f, 0, 1);
            var sb = new StringBuilder();
            sb.Append(ColorizeString("You're playing ", brown));
            sb.Append(ColorizeString($"<u>{HR.SelectedRuleset.Name}</u>", indigo));
            sb.AppendLine(ColorizeString(" ruleset!", brown));
            sb.AppendLine(ColorizeString($"<i>{HR.SelectedRuleset.Description}</i>", Color.blue));
            sb.AppendLine();

            var rulesetPanel = _elementCreator.CreateNewText(sb.ToString());
            rulesetPanel.transform.SetParent(transform, worldPositionStays: false);
            if (numRules < 12)
            {
                rulesetPanel.transform.localPosition = new Vector3(0, 1.25f, VrElementCreator.TextZShift);
            }
            else if (numRules < 24)
            {
                rulesetPanel.transform.localPosition = new Vector3(0, 3.5f, VrElementCreator.TextZShift);
            }
            else
            {
                rulesetPanel.transform.localPosition = new Vector3(0, 7.25f, VrElementCreator.TextZShift);
            }

            sb.Clear();
            int total = 0;
            sb.AppendLine();
            sb.AppendLine(ColorizeString($"<========== {numRules} Active Rules ==========>", Color.magenta));
            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    var isHidden = rule is IHidden;
                    if (isHidden)
                    {
                        total++;
                        continue;
                    }
                    else
                    {
                        var description = HR.SelectedRuleset.Rules[total].Description;
                        sb.AppendLine(ColorizeString($"- {description}", Color.black));
                        total++;
                    }
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    ConfigurationMod.Logger.Warning($"Failed to successfully call on rule [{rule.GetType()}]: {e}");
                }
            }

            var detailsPanel = _elementCreator.CreateLeftText(sb.ToString());
            detailsPanel.transform.SetParent(transform, worldPositionStays: false);
            if (numRules < 12)
            {
                detailsPanel.transform.localPosition = new Vector3(0, -1f, VrElementCreator.TextZShift);
            }
            else if (numRules < 24)
            {
                detailsPanel.transform.localPosition = new Vector3(0, -5f, VrElementCreator.TextZShift);
            }
            else
            {
                detailsPanel.transform.localPosition = new Vector3(0, -10f, VrElementCreator.TextZShift);
            }

            var versionText = _elementCreator.CreateNormalText($"v{RevolutionsVersion.Version}");
            versionText.transform.SetParent(transform, worldPositionStays: false);
            if (numRules < 12)
            {
                versionText.transform.localPosition = new Vector3(-7, -9.5f, VrElementCreator.TextZShift);
            }
            else if (numRules < 24)
            {
                versionText.transform.localPosition = new Vector3(-7, -16.25f, VrElementCreator.TextZShift);
            }
            else
            {
                versionText.transform.localPosition = new Vector3(-7, -28.75f, VrElementCreator.TextZShift);
            }

            if (ConfigurationMod.IsUpdateAvailable2)
            {
                var updateText = _elementCreator.CreateNormalText("NEW UPDATE AVAILABLE!");
                updateText.transform.SetParent(transform, worldPositionStays: false);
                if (numRules < 12)
                {
                    updateText.transform.localPosition = new Vector3(5.75f, -9.5f, VrElementCreator.TextZShift);
                }
                else if (numRules < 24)
                {
                    updateText.transform.localPosition = new Vector3(5.75f, -15.5f, VrElementCreator.TextZShift);
                }
                else
                {
                    updateText.transform.localPosition = new Vector3(5.75f, -28.75f, VrElementCreator.TextZShift);
                }
            }

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
