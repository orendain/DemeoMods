namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HoardersRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hoarders";
            const string description = "A large hand size but you may not get them fast enough. (SKIRMISH ONLY)";
            const string longdesc = "";

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
                new CardEnergyFromAttackMultipliedRule(0.9f),
                new CardEnergyFromRecyclingMultipliedRule(1.5f),
                new CardLimitModifiedRule(22));
        }
    }
}
