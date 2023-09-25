namespace HouseRules.Configuration
{
    using System.IO;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    internal static class ExampleRulesetExporter
    {
        private static readonly string ExampleRulesetDirectory = Path.Combine(ConfigManager.RulesetDirectory, "ExampleRulesets");

        internal static void ExportExampleRulesetsIfNeeded()
        {
            // Uncomment to export registered rulesets.
            // ExportRegisteredRuleset();
        }

        private static void ExportRegisteredRuleset()
        {
            Directory.CreateDirectory(ExampleRulesetDirectory);

            foreach (var ruleset in HR.Rulebook.Rulesets)
            {
                var newName = $"(Custom) {ruleset.Name}";
                var rulesetCopy = Ruleset.NewInstance(newName, ruleset.Description, ruleset.Longdesc, ruleset.Rules);
                HouseRulesConfigurationCore.ConfigManager.ExportRuleset(rulesetCopy, ExampleRulesetDirectory);
            }
        }
    }
}
