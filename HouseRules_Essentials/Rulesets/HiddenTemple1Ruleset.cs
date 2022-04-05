namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HiddenTemple1Ruleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hidden Temple: I";
            const string description = "Explore the legends of the first hidden temple.";

            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "ElvenFloor16",// relative size: 2
                "ShopFloor02",
                "ElvenFloor12",// relative size: 5
                "ShopFloor02",
                "ElvenFloor13",// relative size: 4
            });

            return Ruleset.NewInstance(
                name,
                description,
                levelSequenceOverriddenRule);
        }
    }
}
