namespace HouseRules.Configuration
{
    using System.IO;
    using HouseRules.Types;

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
            foreach (var ruleset in HR.Rulebook.Rulesets)
            {
                var clonedNamed = $"(Custom) {ruleset.Name}";
                var rulesetCopy = Ruleset.NewInstance(clonedNamed, ruleset.Description, ruleset.Rules);
                ConfigurationMod.ConfigManager.ExportRuleset(rulesetCopy);
            }
        }
    }
}
