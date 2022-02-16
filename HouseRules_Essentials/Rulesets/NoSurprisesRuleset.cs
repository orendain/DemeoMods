namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class NoSurprisesRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "No Surprises";
            const string description = "Prevents any surprises in the dark or coming through doors.";

            return Ruleset.NewInstance(
                name,
                description,
                new EnemyDoorOpeningDisabledRule(true),
                new EnemyRespawnDisabledRule(true),
        }
    }
}
