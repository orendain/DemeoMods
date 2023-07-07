namespace HouseRules.Configuration.UI
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using Common.UI;
    using Common.UI.Element;
    using Revolutions;
    using UnityEngine;

    internal class HouseRulesUiGameVr3 : MonoBehaviour
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
            transform.position = new Vector3(20.6f, 41.4f, -82.2f);

            gameObject.AddComponent<FaceLocalPlayer>();

            int numRules = 13;
            string longdesc = "You will NOT start with most of the normal Demeo Revolutions abilities and perks. Instead, you will gain them as you LEVEL UP by filling the card mana meter. You start with 1 knockdown instead of 3. Gaining a level heals the party for 2 (if hurt) and will revive downed party members. You will still gain cards when filling the card meter. Discarding cards will only give 20% of the amount indicated (to avoid exploiting levels). If you die and return to pick another character you will start at level 1 again. Party members can see their current levels indicated by a new permanent buff over their heads. Enemy difficulty increases on each new floor.\n\nLevel 1 - Critical hits on your FINAL action function as in normal Demeo Revolutions except the Guardian only gains 1 Action Point instead of 2 (until level 3)\n\nLevel 2 - Gain 1 permanent replenishable ability. Pull, Stealth, Arrow, Zap, Lightning Bolt, Courage Shanty, Feral Charge, and Grapple now cost 0 Action Points\n\nLevel 3 - Gain 1 max health and critical hits restore your 0 Action Cost replenishables and/or Action Points (depending on class) as in normal Demeo Revolutions\n\nLevel 4 - Gain 1 knockdown and critical hits also earns you 30 gold OR heal you for 1 if you're on the last floor\n\nLevel 5 - Gain 1 bonus & max bonus to a stat (Strength or Magic depending on class)\n\nLevel 6 - Gain 2 max health\n\nLevel 7 - Gain 2 bonus & max bonus to Swiftness\n\nLevel 8 - Gain 1 knockdown and a new permanent powerful ability that can only be used once every 7 turns\n\nLevel 9 - Gain 3 max health\n\nLevel 10 - Gain 2 bonus & max bonus to a stat (Strength or Magic depending on class) AND 1 extra Action Point per turn";

            int textLength = longdesc.Length;
            int returnCount = longdesc.Count(f => f == '\n');

            numRules += 1 + ((textLength - 650) / 65);
            ConfigurationMod.Logger.Msg($"{numRules - 11} from text of {textLength}");
            numRules += returnCount / 2;
            ConfigurationMod.Logger.Msg($"{returnCount} from returns");

            var background = new GameObject("Background");
            var scale = 1.5f + (float)(0.09 * (numRules - 11));
            background.AddComponent<MeshFilter>().mesh = _resourceTable.MenuMesh;
            background.AddComponent<MeshRenderer>().material = _resourceTable.MenuMaterial;
            background.transform.SetParent(transform, worldPositionStays: false);
            background.transform.localPosition = new Vector3(0, 0, 0);
            background.transform.localRotation =
                Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.

            background.transform.localScale = new Vector3(4.75f, 1, scale);

            var header = 3.6f + (float)(0.21f * (numRules - 11));
            var headerText = _elementCreator.CreateMenuHeaderText("Perks and Leveling-Up");
            headerText.transform.SetParent(transform, worldPositionStays: false);

            headerText.transform.localPosition = new Vector3(0, header, VrElementCreator.TextZShift);
            var sb = new StringBuilder();
            sb.AppendLine(ColorizeString($"{longdesc}", Color.black));

            var center = (float)(numRules * .05);
            var details = center - (float)(0.245 * numRules);
            var detailsPanel = _elementCreator.CreateLeftText(sb.ToString());
            detailsPanel.transform.SetParent(transform, worldPositionStays: false);

            detailsPanel.transform.localPosition = new Vector3(0, details, VrElementCreator.TextZShift);

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
