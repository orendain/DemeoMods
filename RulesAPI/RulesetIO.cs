namespace RulesAPI
{
    using System;
    using System.Collections.Generic;
    using MelonLoader;

    internal static class RulesetIO
    {
        private const string CategoryName = "CustomRuleset";

        public static void WriteRuleset(Ruleset ruleset)
        {
            var configCategory = MelonPreferences.CreateCategory(CategoryName);

            foreach (var rule in ruleset.Rules)
            {
                if (!(rule is IConfigurableRule configurableRule))
                {
                    RulesAPI.Logger.Warning($"Rule {rule.GetType().Name} is not serializable. Skipping writing rule to config.");
                    continue;
                }

                configCategory.CreateEntry(configurableRule.GetType().Name, configurableRule.ToConfigString());
            }
        }

        internal static Ruleset ReadRuleset()
        {
            var configCategory = MelonPreferences.CreateCategory(CategoryName);

            foreach (var ruleType in Registrar.Instance().RuleTypes)
            {
                configCategory.CreateEntry(ruleType.Name, default_value: string.Empty, dont_save_default: true);
            }

            var rules = new HashSet<Rule>();
            foreach (var entry in configCategory.Entries)
            {
                if (string.IsNullOrEmpty(entry.GetValueAsString()))
                {
                    continue;
                }

                try
                {
                    rules.Add(RuleParser.Parse(entry.Identifier, entry.GetValueAsString()));
                }
                catch (ArgumentException e)
                {
                    RulesAPI.Logger.Warning($"Failed to parse rule configuration with label [{entry.Identifier}]. Skipping parsing rule due to: {e.Message}");
                }
            }

            return Ruleset.NewInstance("CustomRuleset", "A custom configured ruleset.", rules);
        }
    }
}
