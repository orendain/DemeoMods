﻿namespace HouseRules.Configuration.UI
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using Common.UI;
    using Common.UI.Element;
    using HouseRules.Core;
    using UnityEngine;

    internal class HouseRulesUiGameVr : MonoBehaviour
    {
        private VrResourceTable _resourceTable;
        private IElementCreator _elementCreator;
        private Transform _anchor;

        private void Start()
        {
            StartCoroutine(WaitAndInitialize());
        }

        private IEnumerator WaitAndInitialize()
        {
            while (!VrElementCreator.IsReady()
                   || Resources
                       .FindObjectsOfTypeAll<GameObject>()
                       .Count(x => x.name == "~LeanTween") < 1)
            {
                HouseRulesConfigurationBase.LogDebug("UI dependencies not yet ready. Waiting...");
                yield return new WaitForSecondsRealtime(1);
            }

            HouseRulesConfigurationBase.LogDebug("UI dependencies ready. Proceeding with initialization.");

            _resourceTable = VrResourceTable.Instance();
            _elementCreator = VrElementCreator.Instance();
            _anchor = Resources
                .FindObjectsOfTypeAll<GameObject>()
                .First(x => x.name == "~LeanTween").transform;

            Initialize();
            HouseRulesConfigurationBase.LogDebug("Initialization complete.");
        }

        private void Initialize()
        {
            transform.SetParent(_anchor, worldPositionStays: true);
            transform.position = new Vector3(35.6f, 36.4f, -32.2f);

            gameObject.AddComponent<FaceLocalPlayer>();

            int numRules = 13;
            int textLength = 0;
            int returnCount = 0;
            if (!string.IsNullOrEmpty(HR.SelectedRuleset.Longdesc))
            {
                textLength = HR.SelectedRuleset.Longdesc.Length;
                returnCount = HR.SelectedRuleset.Longdesc.Count(f => f == '\n');
            }

            if (textLength < 1)
            {
                numRules = HR.SelectedRuleset.Rules.Count;
            }
            else if (textLength > 650)
            {
                numRules += 1 + ((textLength - 650) / 65);

                if (returnCount > 0)
                {
                    numRules += returnCount / 2;
                }
            }
            else if (returnCount > 10)
            {
                numRules += returnCount - 10;
            }
            else if (returnCount + (textLength / (25 * returnCount)) > 10)
            {
                numRules += returnCount + (textLength / (25 * returnCount)) - 10;
            }

            var background = new GameObject("Background");
            var scale = 1.5f;
            background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            background.transform.SetParent(transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, 0, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
            if (numRules > 11)
            {
                scale = 1.5f + (float)(0.09 * (numRules - 11));
            }

            background.transform.localScale = new Vector3(4.75f, 1, scale);

            var header = 3.6f;
            var headerText = _elementCreator.CreateMenuHeaderText("HouseRules");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            if (numRules > 11)
            {
                header = 3.6f + (float)(0.21f * (numRules - 11));
            }

            headerText.transform.localPosition = new Vector3(0, header, VrElementCreator.TextZShift);
            Color indigo = new Color(0.294f, 0f, 0.51f);
            Color brown = new Color(0.0392f, 0.0157f, 0, 1);
            var sb = new StringBuilder();
            sb.Append(ColorizeString("You're playing ", brown));
            sb.Append(ColorizeString($"<u>{HR.SelectedRuleset.Name}</u>", indigo));
            sb.AppendLine(ColorizeString(" ruleset!", brown));
            sb.AppendLine(ColorizeString($"<i>{HR.SelectedRuleset.Description}</i>", Color.blue));
            sb.AppendLine();
            var ruleset = 1.25f;
            var drift = 1.25f;
            var rulesetPanel = _elementCreator.CreateNewText(sb.ToString());
            rulesetPanel.transform.SetParent(transform, worldPositionStays: false);
            if (numRules > 40)
            {
                drift = 1.25f - (float)(0.035f * (numRules - 20));
            }
            else if (numRules > 20)
            {
                drift = 1.25f - (float)(0.045f * (numRules - 20));
            }

            if (numRules > 11)
            {
                ruleset = drift + (float)(0.2f * (numRules - 11));
            }

            rulesetPanel.transform.localPosition = new Vector3(0, ruleset, VrElementCreator.TextZShift);

            sb.Clear();
            if (textLength > 0)
            {
                sb.AppendLine(ColorizeString($"<========== Ruleset Creator's Description ==========>", Color.white));
                sb.AppendLine(ColorizeString($"{HR.SelectedRuleset.Longdesc}", Color.black));
            }
            else
            {
                int total = 0;
                sb.AppendLine();
                if (numRules > 1)
                {
                    sb.AppendLine(ColorizeString($"<========== {numRules} Active Rules ==========>", Color.white));
                }
                else
                {
                    sb.AppendLine(ColorizeString($"<========== 1 Active Rule ==========>", Color.white));
                }

                foreach (var rule in HR.SelectedRuleset.Rules)
                {
                    try
                    {
                        var description = HR.SelectedRuleset.Rules[total].Description;
                        sb.AppendLine(ColorizeString($"- {description}", Color.black));
                        total++;
                    }
                    catch (Exception e)
                    {
                        // TODO(orendain): Consider rolling back or disable rule.
                        HouseRulesConfigurationBase.LogWarning($"Failed to successfully call on rule [{rule.GetType()}]: {e}");
                    }
                }
            }

            var center = -2f + (float)(numRules * .05);
            var details = center - (float)(0.245 * numRules);
            var detailsPanel = _elementCreator.CreateLeftText(sb.ToString());
            detailsPanel.transform.SetParent(transform, worldPositionStays: false);
            detailsPanel.transform.localPosition = new Vector3(0, details, VrElementCreator.TextZShift);

            sb.Clear();
            sb.Append(ColorizeString($"v{BuildVersion.Version}", Color.yellow));
            var version = -9.5f;
            var versionText = _elementCreator.CreateNormalText(sb.ToString());
            versionText.transform.SetParent(transform, worldPositionStays: false);
            if (numRules > 11)
            {
                version = -9.5f - (float)(0.58 * (numRules - 11));
            }

            versionText.transform.localPosition = new Vector3(-7, version, VrElementCreator.TextZShift);

            if (HouseRulesConfigurationBase.IsUpdateAvailable)
            {
                sb.Clear();
                sb.Append(ColorizeString("NEW UPDATE AVAILABLE", Color.green));
                var updateText = _elementCreator.CreateNormalText(sb.ToString());
                updateText.transform.SetParent(transform, worldPositionStays: false);
                updateText.transform.localPosition = new Vector3(5.75f, version, VrElementCreator.TextZShift);
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
