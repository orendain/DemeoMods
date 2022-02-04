namespace RulesAPI
{
    using System;
    using System.Collections.Generic;
    using HarmonyLib;
    using MelonLoader;

    public static class RulesetIO
    {
        private const string CategoryName = "CustomRuleset";

        public static void WriteRuleset(Ruleset ruleset)
        {
            var configCategory = MelonPreferences.CreateCategory(CategoryName);

            foreach (var rule in ruleset.Rules)
            {
                if (!(rule is IConfigWritable writableRule))
                {
                    RulesAPI.Logger.Warning($"Rule {rule.GetType().FullName} does not implement IConfigWritable. Skipping writing rule.");
                    continue;
                }

                configCategory.CreateEntry(writableRule.GetType().Name, writableRule.ToConfigString());
            }
        }

        public static Ruleset ReadRuleset()
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
                    var rule = InstantiateRule(entry.Identifier, entry.GetValueAsString());
                    rules.Add(rule);
                }
                catch (ArgumentException e)
                {
                    RulesAPI.Logger.Warning($"Failed to parse rule configuration with label [{entry.Identifier}]. Skipping rule due to: {e.Message}");
                }
            }

            return Ruleset.NewInstance("CustomRuleset", "A custom configured ruleset.", rules);
        }

        /// <summary>
        /// Instantiates a rule given the rule name and its configuration string.
        /// </summary>
        /// <param name="ruleName">The name of the rule.</param>
        /// <param name="configString">The string representing the rule's configuration.</param>
        /// <returns>An instantiated rule configured with the specified configuration.</returns>
        /// <exception cref="ArgumentException">If the specified rule could not be found, or is not configurable.</exception>
        private static Rule InstantiateRule(string ruleName, string configString)
        {
            var typ = AccessTools.TypeByName(ruleName);
            if (typ == null || !typeof(Rule).IsAssignableFrom(typ))
            {
                throw new ArgumentException($"Could not find a rule type corresponding to rule name: {ruleName}");
            }

            if (!typeof(IConfigWritable).IsAssignableFrom(typ))
            {
                throw new ArgumentException($"Could not recognize rule type as implementing IConfigWritable: {typ.FullName}");
            }

            var traverse = Traverse.Create(typ).Method("FromConfigString", paramTypes: new[] { typeof(string) }, arguments: new object[] { configString });
            if (!traverse.MethodExists())
            {
                throw new ArgumentException($"Could not find expected FromConfigString method for rule type: {typ.FullName}");
            }

            return traverse.GetValue<Rule>();
        }
    }
}
