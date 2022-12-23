namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;
    using System.Collections.Generic;

    internal static class NoSurprisesRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "No Surprises";
            const string description = "No surprises in the dark or coming through doors.";

            return Ruleset.NewInstance(
                name,
                description,
                new EnemyDoorOpeningDisabledRule(true),
                new EnemyRespawnDisabledRule(true));
                new LevelPropertiesModifiedRule(new Dictionary<string, int>
                {
                    { "FloorOneElvenSummoners", 0 },
                    { "FloorTwoElvenSummoners", 0 },
                    { "FloorThreeElvenSummoners", 0 },
                });
        }
    }
}
