namespace HouseRules.Configuration
{
    using HouseRules.Core;
    using HouseRules.Core.Types;

    internal static class ExampleRulesetExporter
    {
        internal static void ExportRegisteredRulesets(string directory)
        {
            var configManager = ConfigManager.NewInstance(directory);
            foreach (var ruleset in HR.Rulebook.Rulesets)
            {
                var newName = $"(Custom) {ruleset.Name}";
                var rulesetCopy = Ruleset.NewInstance(newName, ruleset.Description, ruleset.Longdesc, ruleset.Rules);
                configManager.ExportRuleset(rulesetCopy, directory);
            }
        }
    }
}
