namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HiddenForestRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hidden Forest";
            const string description = "A never-before seen forest full of mysteries.";

            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "ForestShopFloor",
                "ForestFloor07", //medlarge ... maybe seen before...
                "ForestShopFloor",
                "ForestFloor08", //med/big
            });


            return Ruleset.NewInstance(
                name,
                description,
                levelSequenceOverriddenRule);
        }
    }
}
