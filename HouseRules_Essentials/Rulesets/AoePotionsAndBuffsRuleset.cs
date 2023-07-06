namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class AoePotionsAndBuffsRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "3x3 Potions and Buffs";
            const string description = "Heal, Strength, Speed, Adamant, Vitality, Damage Resist, Focus, One More Thing, Antitoxin, Repair Armor and Bard buffs are 3x3 AOE.";
            const string longdesc = "";

            var abilityAoeRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.CourageShanty, 1 },
                { AbilityKey.ReplenishArmor, 1 },
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.OneMoreThing, 1 },
                { AbilityKey.Antitoxin, 1 },
                { AbilityKey.AdamantPotion, 1 },
                { AbilityKey.HealingPotion, 1 },
            });

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.StrengthPotion, false },
                { AbilityKey.SwiftnessPotion, false },
                { AbilityKey.VigorPotion, false },
                { AbilityKey.DamageResistPotion, false },
                { AbilityKey.OneMoreThing, false },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneElvenSummoners", 0 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeElvenSummoners", 0 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                levelPropertiesRule,
                abilityAoeRule,
                abilityMaxedRule);
        }
    }
}
