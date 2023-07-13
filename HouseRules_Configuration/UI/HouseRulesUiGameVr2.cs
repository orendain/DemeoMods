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

    internal class HouseRulesUiGameVr2 : MonoBehaviour
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
            transform.position = new Vector3(30.6f, 41.4f, -57.2f);

            gameObject.AddComponent<FaceLocalPlayer>();

            string longdesc = string.Empty;
            int numRules = 8;
            if (HR.SelectedRuleset.Name.Contains("PROGRESSIVE"))
            {
                longdesc = "- Guardian: 8 max health. Immune to Weaken and non-boss Fire damage. Normal start cards plus Charge and War Cry. Replenish Armor regains 4 armor instead of 5. War Cry and Whirlwind AoE changed from 3x3 to 5x5. War Cry now also blinds enemies as well as panicking them. Whirlwind doesn't harm allies but it will still knock them back. Critical hits on LAST action regain 1 AP [max 2 AP at level 3], 1 Armor, Pull [at level 3], and gain Berserk (+1 Damage & +3 Movement for the turn). Enemies that melee you are counter-attacked for 1 damage\n\n- Barbarian: 7 max health. Immune to Petrify and Slime damage. Starting critical damage increased to 9. Normal start cards plus The Leviathan and Barbaric Chainwhip. Barbaric Chainwhip damage increased to 3. Howl of the Ancients now also confuses enemies as well as weakening them. Grapple costs 0 AP. Grapple and Launching a Lamp can be used in the same turn. Critical hits on the LAST action gain 6 Vargas, 1 Magic Armor [max 2 Magic Armor at level 3], and a Spawn Random Lamp card. Having a Mark of Varga card puts you closer to the front in turn order\n\n- Hunter: 7 max health. You and Verochka are immune to being Frozen and non-boss Ice damage. Starting melee damage reduced to 2 (ranged damage is always 1 higher than melee). Starting critical RANGED damage increased to 8. Starting movement increased to 5. Normal start cards plus Call Companion and Lure. Beast Whisperer summoned Rats are now poisonous. Critical hits on LAST action gain a Bone, a Freeze Arrow and 6 rounds of Fire Immunity. Having a Hunter's Mark card puts you closer to the front in turn order\n\n- Assassin: 7 max health. Immune to being Tangled and Netted. Starting critical damage increased to 8. Starting movement increased to 5. Normal start cards plus Cursed Dagger and Flash Bomb. Flash Bomb area coverage changed from 3x3 to 5x5. Poison Bomb and Flash Bomb do not break Sneak. All attacks have 50% chance to heal you for 1 (if hurt). Critical hits ALWAYS heal you for 1 and have 50% chance to heal you for 2 (if hurt). Sneaking now adds damage to normal melee and ranged attacks. Critical hits on LAST action gain Invisibility that lasts until the end of the NEXT round\n\n- Bard: 7 max health. Immune to Poison and being Blinded. Starting melee damage reduced to 2. Can now backstab (and so can your damaging songs). Courage Shanty has 33% chance to heal/revive the target for 1 (if hurt/downed). Recovery and Resilience songs AoE changed from 3x3 to 7x7. Normal start cards plus Shattering Voice and Piercing Voice. A NEW single-target blinding Flashbomb card that doesn't miss and is reusable every 3 rounds. Critical hits heal you for 1 (if hurt) and reduce Flashbomb cooldown by 1. Critical hits on LAST action gain Panic Powder, Courageous self-buff or upgrade, 2 rounds of Deflection shield and +3 movement until the shield fades\n\n- Warlock: 6 max health. You and Cána are immune to being Corrupted and Corruption damage. Starting melee damage reduced to 1. Cána starts with 1 point of innate damage resist (can NOT be increased). If Cána goes half life or under she gains Frenzy (+1 damage until life goes over half). Normal start cards plus Consuming Vortex and Astral Barrier. Astral Barrier AoE changed from 3x3 to 7x7. Enemies that hit you or Cána will become Astral Marked for 1 turn. All your damaging attacks on Astral Marked targets heal you for 1 (if hurt). Critical hits on LAST action gain +3 Magic damage and Astral Barrier until the end of the NEXT round. Having an Astral Barrier card puts you closer to the front in turn order\n\n- Sorcerer: 6 max health. Immune to Stun and non-boss Electricity damage. Starting melee damage reduced to 1. Starts with 1 Magic Bonus and maximum Magic Bonus of 6. Normal start cards plus Vortex and Banish. Any electrical attacks you use won't damage or stun other players or non-charmed allies. Critical hits on LAST action gain Water Bottle and become Overcharged";
            }
            else if (HR.SelectedRuleset.Name.Contains("EASY"))
            {
                longdesc = "- Guardian: 10 max health. Immune to Weaken and non-boss Fire damage. Normal start cards plus Charge and War Cry. War Cry and Whirlwind AoE changed from 3x3 to 5x5. War Cry now also blinds enemies as well as panicking them. Whirlwind doesn't harm allies but it will still knock them back. Critical hits regain AP (2 max), 1 Armor, Pull and gain Berserk (+1 Damage & +3 Movement for the turn). Enemies that melee you are counter-attacked for 1 damage\n\n- Barbarian: 9 max health. Immune to Petrify and Slime damage. Starting critical damage increased to 9. Normal start cards plus The Leviathan and Barbaric Chainwhip. Barbaric Chainwhip damage increased to 3. Howl of the Ancients now also confuses enemies as well as weakening them. Grapple costs 0 AP. Grapple and launching a lamp can be used in the same turn. Critical hits regain Grappleand gain 1 Magic Armor. Critical hits on the LAST action gain 6 Vargas, 2 Magic Armor, and a Spawn Random Lamp card. Having a Mark of Varga card puts you closer to the front in turn order\n\n- Hunter: 9 max health. You and Verochka are immune to being Frozen and non-boss Ice damage. Starting melee damage reduced to 2 (ranged damage is always 1 higher than melee). Starting critical RANGED damage increased to 8. Starting movement increased to 5. Normal start cards plus Call Companion and Lure. Beast Whisperer summoned Rats are now poisonous. Critical hits regain Arrow. Critical hits on LAST action gain a Bone, a Freeze Arrow and 6 rounds of Fire Immunity. Having a Hunter's Mark card puts you closer to the front in turn order\n\n- Assassin: 9 max health. Immune to being Tangled and Netted. Starting critical damage increased to 8. Starting movement increased to 5. Normal start cards plus Cursed Dagger and Flash Bomb. Flash Bomb area coverage changed from 3x3 to 5x5. Poison Bomb and Flash Bomb do not break Sneak. All attacks have 50% chance to heal you for 1 (if hurt). Critical hits regain 1 AP, ALWAYS heal you for 1 and have 50% chance to heal you for 2 (if hurt). Critical hits on LAST action gain Invisibility that lasts until the end of the NEXT round. Sneaking now adds damage to normal melee and ranged attacks\n\n- Bard: 9 max health. Immune to Poison and being Blinded. Starting melee damage reduced to 2. Can now backstab (and so can your damaging songs). Courage Shanty has 33% chance to heal/revive the target for 1 (if hurt/downed). Recovery and Resilience songs AoE changed from 3x3 to 7x7. Normal start cards plus Shattering Voice and Piercing Voice. A NEW single-target blinding Flashbomb card that doesn't miss and is reusable every 3 rounds. Critical hits regain Courage Shanty, gain a Courageous self-buff, heal you for 1 (if hurt) and reduce Flashbomb cooldown by 1. Critical hits on LAST action gain Panic Powder, Courageous self-buff or upgrade, 2 rounds of Deflection shield and +3 movement until the shield fades\n\n- Warlock: 8 max health. You and Cána are immune to being Corrupted and Corruption damage. Starting melee damage reduced to 1. Cána starts with 1 point of innate damage resist (can NOT be increased). If Cána goes half life or under she gains Frenzy (+1 damage until life goes over half). Normal start cards plus Consuming Vortex and Astral Barrier. Astral Barrier AoE changed from 3x3 to 7x7. Enemies that hit you or Cána will become Astral Marked for 1 turn. All your damaging attacks on Astral Marked targets heal you for 1 (if hurt). Critical hits regain Feral Charge. Critical hits on LAST action gain +3 Magic damage and Astral Barrier until the end of the NEXT round. Having an Astral Barrier card puts you closer to the front in turn order\n\n- Sorcerer: 8 max health. Immune to Stun and non-boss Electricity damage. Starting melee damage reduced to 1. Starts with 1 Magic Bonus and maximum Magic Bonus of 6. Normal start cards plus Vortex and Banish. Any electrical attacks you use won't damage or stun other players or non-charmed allies. Critical hits regain Zap (or if Overcharged gain Water Bottle). Critical hits on LAST action gain Water Bottle and become Overcharged";
            }
            else if (HR.SelectedRuleset.Name.Contains("LEGENDARY") || HR.SelectedRuleset.Name.Contains("HARD"))
            {
                longdesc = "- Guardian: 8 max health. Immune to Weaken and non-boss Fire damage. Normal start cards plus Charge and War Cry. Replenish Armor regains 4 armor instead of 5. War Cry and Whirlwind AoE changed from 3x3 to 5x5. War Cry now also blinds enemies as well as panicking them. Whirlwind doesn't harm allies but it will still knock them back. A NEW reusable non-damaging Pull card which costs 0 AP (works on enemies and allies). Critical hits regain AP (2 max), 1 Armor, Pull and gain Berserk (+1 Damage & +3 Movement for the turn). Enemies that melee you are counter-attacked for 1 damage\n\n- Barbarian: 7 max health. Immune to Petrify and Slime damage. Starting critical damage increased to 9. Normal start cards plus The Leviathan and Barbaric Chainwhip. Barbaric Chainwhip damage increased to 3. Howl of the Ancients now also confuses enemies as well as weakening them. Grapple costs 0 AP. Grapple and Launching a Lamp can now be used in the same turn. A NEW non-damaging Net card that roots an enemy in place and can be reused every 3 rounds. Critical hits regain Grapple, reduce Net cooldown by 1, and gain 1 Magic Armor if not last action. Critical hits on the LAST action gain 6 Vargas, 2 Magic Armor, and a Spawn Random Lamp card. Having a Mark of Varga card puts you closer to the front in turn order\n\n- Hunter: 7 max health. You and Verochka are immune to being Frozen and non-boss Ice damage. Starting melee damage reduced to 2 (ranged damage is always 1 higher than melee). Starting critical RANGED damage increased to 8. Starting movement increased to 5. Arrow costs 0 AP. Normal start cards plus Call Companion and Lure. Beast Whisperer summoned Rats are now poisonous. Your second Arrow card is replaced by a NEW reusable single target Fire Arrow attack that can't miss. Critical hits replenish Arrow if not last action. Critical hits on LAST action gain a Bone, a Freeze Arrow and gain 6 rounds of Fire Immunity. Having a Hunter's Mark card puts you closer to the front in turn order\n\n- Assassin: 7 max health. Immune to being Tangled and Netted. Starting critical damage increased to 8. Starting movement increased to 5, Sneak costs 0 AP. Normal start cards plus Cursed Dagger and Flash Bomb. Flash Bomb area coverage changed from 3x3 to 5x5. A NEW ranged Poison attack card that can't miss, can backstab, and is reusable every 3 rounds. Poison Bomb, Flash Bomb and the NEW ranged Poison attack do not break Sneak. All attacks have 50% chance to heal you for 1 (if hurt). Critical hits ALWAYS heal you for 1 and have 50% chance to heal you for 2 (if hurt). Sneaking now adds damage to normal melee and ranged attacks. Critical hits regain 1 AP if not last action and reduce ranged Poison attack cooldown by 1. Critical hits on LAST action gain Invisibility that lasts until the end of the NEXT round\n\n- Bard: 7 max health. Immune to Poison and being Blinded. Starting melee damage reduced to 2. Can now backstab (and so can your damaging songs). Courage Shanty costs 0 AP and has 33% chance to heal/revive the target for 1 (if hurt/downed). Recovery and Resilience songs AoE changed from 3x3 to 7x7. Normal start cards plus Shattering Voice and Piercing Voice. A NEW single-target blinding Flashbomb card that doesn't miss and is reusable every 3 rounds. Critical hits heal you for 1 (if hurt) and reduce Flashbomb cooldown by 1. Critical hits replenish Courage Shanty if not last action and give you a Courageous self-buff or upgrade. Critical hits on LAST action ALSO gain Panic Powder and 2 rounds of Deflection shield and movement +3\n\n- Warlock: 6 max health. You and Cána are immune to being Corrupted and Corruption damage. Starting melee damage reduced to 1. Cána starts with 1 point of innate damage resist (can NOT be increased). If Cána goes half life or under she gains Frenzy (+1 damage until life goes over half). Normal start cards plus Consuming Vortex, Astral Barrier and a replenishable Feral Charge that costs 0 AP. Astral Barrier AoE changed from 3x3 to 7x7. Enemies that hit you or Cána will become Astral Marked for 1 turn. All your damaging attacks on Astral Marked targets heal you for 1 (if hurt). Critical hits replenish Feral Charge if not last action. Critical hits on LAST action gain +3 Magic damage and Astral Barrier until the end of the NEXT round. Having an Astral Barrier card puts you closer to the front in turn order\n\n- Sorcerer: 6 max health. Immune to Stun and non-boss Electricity damage. Starting melee damage reduced to 1. Starts with 1 Magic Bonus and maximum Magic Bonus of 6. Zap and Lightning Bolt cost 0 AP. Normal start cards plus Vortex and Banish. Overcharge is replaced by a NEW reusable arcing Thunderbolt attack card. Any electrical attacks you use won't damage or stun other players or non-charmed allies. Critical hits replenish Zap (NOT Lightning Bolt). Critical hits on LAST action gain Water Bottle and become Overcharged. Critical hits while NOT Overcharged replenish Zap if not last action. Critical hits while Overcharged gain Water Bottle";
            }
            else
            {
                longdesc = "- Guardian: 9 max health. Immune to Weaken and non-boss Fire damage. Normal start cards plus Charge and War Cry. War Cry and Whirlwind AoE changed from 3x3 to 5x5. War Cry now also blinds enemies as well as panicking them. Whirlwind doesn't harm allies but it will still knock them back. A NEW reusable non-damaging Pull card which costs 0 AP (works on enemies and allies). Critical hits regain AP (2 max), 1 Armor, Pull and gain Berserk (+1 Damage & +3 Movement for the turn). Enemies that melee you are counter-attacked for 1 damage\n\n- Barbarian: 8 max health. Immune to Petrify and Slime damage. Starting critical damage increased to 9. Normal start cards plus The Leviathan and Barbaric Chainwhip. Barbaric Chainwhip damage increased to 3. Howl of the Ancients now also confuses enemies as well as weakening them. Grapple costs 0 AP. Grapple and Launching a Lamp can now be used in the same turn. A NEW non-damaging Net card that roots an enemy in place and can be reused every 3 rounds. Critical hits regain Grapple, reduce Net cooldown by 1, and gain 1 Magic Armor if not last action. Critical hits on the LAST action gain 6 Vargas, 2 Magic Armor, and a Spawn Random Lamp card. Having a Mark of Varga card puts you closer to the front in turn order\n\n- Hunter: 8 max health. You and Verochka are immune to being Frozen and non-boss Ice damage. Starting melee damage reduced to 2 (ranged damage is always 1 higher than melee). Starting critical RANGED damage increased to 8. Starting movement increased to 5. Arrow costs 0 AP. Normal start cards plus Call Companion and Lure. Beast Whisperer summoned Rats are now poisonous. Your second Arrow card is replaced by a NEW reusable single target Fire Arrow attack that can't miss. Critical hits replenish Arrow if not last action. Critical hits on LAST action gain a Bone, a Freeze Arrow and gain 6 rounds of Fire Immunity. Having a Hunter's Mark card puts you closer to the front in turn order\n\n- Assassin: 8 max health. Immune to being Tangled and Netted. Starting critical damage increased to 8. Starting movement increased to 5, Sneak costs 0 AP. Normal start cards plus Cursed Dagger and Flash Bomb. Flash Bomb area coverage changed from 3x3 to 5x5. A NEW ranged Poison attack card that can't miss, can backstab, and is reusable every 3 rounds. Poison Bomb, Flash Bomb and the NEW ranged Poison attack do not break Sneak. All attacks have 50% chance to heal you for 1 (if hurt). Critical hits ALWAYS heal you for 1 and have 50% chance to heal you for 2 (if hurt). Sneaking now adds damage to normal melee and ranged attacks. Critical hits regain 1 AP if not last action and reduce ranged Poison attack cooldown by 1. Critical hits on LAST action gain Invisibility that lasts until the end of the NEXT round\n\n- Bard: 8 max health. Immune to Poison and being Blinded. Starting melee damage reduced to 2. Can now backstab (and so can your damaging songs). Courage Shanty costs 0 AP and has 33% chance to heal/revive the target for 1 (if hurt/downed). Recovery and Resilience songs AoE changed from 3x3 to 7x7. Normal start cards plus Shattering Voice and Piercing Voice. A NEW single-target blinding Flashbomb card that doesn't miss and is reusable every 3 rounds. Critical hits heal you for 1 (if hurt) and reduce Flashbomb cooldown by 1. Critical hits replenish Courage Shanty if not last action and give you a Courageous self-buff or upgrade. Critical hits on LAST action ALSO gain Panic Powder and 2 rounds of Deflection shield and movement +3\n\n- Warlock: 7 max health. You and Cána are immune to being Corrupted and Corruption damage. Starting melee damage reduced to 1. Cána starts with 1 point of innate damage resist (can NOT be increased). If Cána goes half life or under she gains Frenzy (+1 damage until life goes over half). Normal start cards plus Consuming Vortex, Astral Barrier and a replenishable Feral Charge that costs 0 AP. Astral Barrier AoE changed from 3x3 to 7x7. Enemies that hit you or Cána will become Astral Marked for 1 turn. All your damaging attacks on Astral Marked targets heal you for 1 (if hurt). Critical hits replenish Feral Charge if not last action. Critical hits on LAST action gain +3 Magic damage and Astral Barrier until the end of the NEXT round. Having an Astral Barrier card puts you closer to the front in turn order\n\n- Sorcerer: 7 max health. Immune to Stun and non-boss Electricity damage. Starting melee damage reduced to 1. Starts with 1 Magic Bonus and maximum Magic Bonus of 6. Zap and Lightning Bolt cost 0 AP. Normal start cards plus Vortex and Banish. Overcharge is replaced by a NEW reusable arcing Thunderbolt attack card. Any electrical attacks you use won't damage or stun other players or non-charmed allies. Critical hits replenish Zap (NOT Lightning Bolt). Critical hits on LAST action gain Water Bottle and become Overcharged. Critical hits while NOT Overcharged replenish Zap if not last action. Critical hits while Overcharged gain Water Bottle";
            }

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

            var headerText = _elementCreator.CreateMenuHeaderText("Character Class Changes");
            headerText.transform.SetParent(transform, worldPositionStays: false);
            var header = 3.6f + (float)(0.21f * (numRules - 11));

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
