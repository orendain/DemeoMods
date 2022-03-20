namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class BetterSorcererRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Better Sorcerer";
            const string description = "0 Action Cost for Sorcerer's Zap - No other changes. #STS";

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
            });

            return Ruleset.NewInstance(
                name,
                description,
                abilityActionCostRule);
        }
    }
}
