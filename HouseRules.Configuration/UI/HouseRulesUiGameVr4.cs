namespace HouseRules.Configuration.UI
{
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Common.UI;
    using Common.UI.Element;
    using UnityEngine;

    internal class HouseRulesUiGameVr4 : MonoBehaviour
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
            transform.position = new Vector3(7f, 41.4f, -53f);
            transform.rotation = Quaternion.Euler(0, 180, 0);

            int numRules = 13;

            // Heroes Progressive Mod Perks and Leveling.
            string longdesc = "Heroes of the Voice Machine\n\nHeroes start at level 1 and will gain Abilities and Stats as they LEVEL UP by filling the card mana meter. You start with 2 knockdowns instead of 3. When knocked down you MUST be picked up by magic, a potion, or a fountain or you lose a level! If you lose a level you will gain a couple of visual buffs letting you know.\n\nGaining a level (by filling the blue card mana meter) heals the party for 2 (if hurt) and will revive downed party members without the level penalty. You will still gain cards when filling the card meter.\nDiscarding cards will only give a fraction of the amount indicated (to avoid exploiting levels). If you die and return to pick another character you will start at level 1 again. Party members can see their current levels indicated by a new permanent buff over their heads. Points Rule is enabled and shows above each hero. Points here are for Co-op (not PvP) and are for bragging rights only.\nHitting Lamps grants an immunity to its damage or a corresponding short term benefit.\nPlaceable objects cause effects when hit: HealingBeacon/Recovery, Lure/Confused, Barricades/Thorns, Eye/Revealed, Leviathan/Stun. Enemy difficulty increases on each new floor.\n\nAt certain levels each Hero gains a special team power up card. Use the special card in coordination with the other Heroes to gain even more powerful benefits.\n\n** * Leveling * **\n\nLevel 1 - Limited starting cards, but everyone gets a Health Potion!\n\nLevel 2 - Heroes gain max Health\nHeroes upgrade a refreshing ability to be 0 Action Cost: Master's Call, Courage, Blinding Light, Whip, Sneak, Howl of the Ancients, Serpent's Blast\n\nLevel 3 - Heroes gain their class specific tier 2 refreshing abilities and some one time use cards.\n\nLevel 4 - Heroes gain 1 knockdown and max Health.\n\nLevel 5 - Heroes gain a boost to stats and a powerful one time use card.\n\nLevel 6 - Heroes gain class specific stats and their tier 3 refreshing abilities and some one time use cards.\n\nLevel 7 - Downed Counter increases and gain more Speed. Cana becomes more powerful.\n\nLevel 8 - Heroes gain a new ability that can only be used once every 5 turns. Abilities are random, not based on class.\n\nLevel 9 - Heroes gain boost to stats and now have 1 health regeneration per turn (except if downed, poisoned, or petrified).\n\nLevel 10 - Heroes gain 1 extra Action Point per turn.\n\nLevel 11 and up - Yes, levels keep going! Stats, buffs, and other benefits at each new level!";

            int textLength = longdesc.Length;

            // Count how many return characters are in the string
            int returnCount = longdesc.Count(f => f == '\n');
            foreach (Match match in Regex.Matches(longdesc, "<color=", RegexOptions.None))
            {
                // Subtract colorization code from textLength
                textLength -= 23;
            }

            foreach (Match match in Regex.Matches(longdesc, "\n\n", RegexOptions.None))
            {
                // Subtract 1 from returnCount if double return characters
                returnCount -= 1;
            }

            numRules += ((textLength - 650) / 65) + returnCount;

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
            var headerText = _elementCreator.CreateMenuHeaderText("<color=#FFF700>Perks and Leveling-Up</color>");
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
