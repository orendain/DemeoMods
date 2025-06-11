namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class AoePotionsAndBuffsRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "3x3 Potions and Buffs";
            const string description = "Heal, Strength, Speed, Adamant, Vitality, Damage Resist, Focus, One More Thing, Antidote, Repair Armor and Bard buffs are 3x3 AOE.";
            const string longdesc = "";

            var abilityAoeRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.StrengthenCourage, 1 },
                { AbilityKey.ReplenishArmor, 1 },
                { AbilityKey.Strength, 1 },
                { AbilityKey.Speed, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.FreeAP, 1 },
                { AbilityKey.Antidote, 1 },
                { AbilityKey.Invulnerability, 1 },
                { AbilityKey.Heal, 1 },
            });

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Strength, false },
                { AbilityKey.Speed, false },
                { AbilityKey.VigorPotion, false },
                { AbilityKey.DamageResistPotion, false },
                { AbilityKey.ExtraActionPotion, false },
                { AbilityKey.FreeAP, false },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                abilityAoeRule,
                abilityMaxedRule);
        }
    }
}
