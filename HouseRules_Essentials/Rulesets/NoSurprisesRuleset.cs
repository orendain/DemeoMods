namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class NoSurprisesRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "No Surprises";
            const string description = "No surprises in the dark or coming through doors.";
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
                new EnemyDoorOpeningDisabledRule(true),
                new EnemyRespawnDisabledRule(true));
        }
    }
}
