namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HiddenTemple2Ruleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hidden Temple: II";
            const string description = "Explore the legends of the second hidden temple.";

            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "ElvenFloor14",// relative size: 1
                "ShopFloor02",
                "ForestFloor08", // relative size: 6
                "ForestShopFloor",
                "ElvenFloor17",// relative size: 3
            });


            return Ruleset.NewInstance(
                name,
                description,
                levelSequenceOverriddenRule);
        }
    }
}
