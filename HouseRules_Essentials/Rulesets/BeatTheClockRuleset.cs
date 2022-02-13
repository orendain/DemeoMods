namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class BeatTheClockRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Beat The Clock!";
            const string description = "Godmode. Ultra recycling. 40 rounds to beat the game.";

            var healthRule = new StartHealthAdjustedRule(new Dictionary<string, int>
            {
                { "HeroGuardian", 990 },
                { "HeroSorcerer", 990 },
                { "HeroRouge", 990 },
                { "HeroHunter", 990 },
                { "HeroBard", 990 },
            });
            var recyclingRule = new CardEnergyFromRecyclingMultipliedRule(6);
            var roundLimitRule = new RoundCountLimitedRule(40);

            return Ruleset.NewInstance(
                name,
                description,
                healthRule,
                recyclingRule,
                roundLimitRule);
        }
    }
}
