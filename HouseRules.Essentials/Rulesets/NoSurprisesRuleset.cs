namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class NoSurprisesRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "No Surprises";
            const string description = "No surprises in the dark or coming through doors.";
            const string longdesc = "";

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                new EnemyDoorOpeningDisabledRule(true),
                new EnemyRespawnDisabledRule(true));
        }
    }
}
