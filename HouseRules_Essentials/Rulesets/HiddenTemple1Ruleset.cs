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
                "ElvenFloor14",//small
                "ShopFloor02",
                "ElvenFloor13",//medium
                "ShopFloor02",
                "ElvenFloor12",// big
            });

            return Ruleset.NewInstance(
                name,
                description,
                levelSequenceOverriddenRule);
        }
    }
}
