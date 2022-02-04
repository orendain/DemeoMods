namespace RulesAPI
{
    using System;
    using System.Collections.Generic;
    using HarmonyLib;
    using MelonLoader;

    public class ConfigManager
    {
        private readonly MelonPreferences_Category _configCategory;
        private readonly MelonPreferences_Entry<string> _selectedRulesetEntry;

        internal static ConfigManager NewInstance()
        {
            return new ConfigManager();
        }

        private ConfigManager()
        {
            _configCategory = MelonPreferences.CreateCategory("RulesAPI");
            _selectedRulesetEntry = _configCategory.CreateEntry("ruleset", string.Empty);
        }

        internal void SaveSelectedRuleset(string rulesetName)
        {
            _selectedRulesetEntry.Value = rulesetName;
            _configCategory.SaveToFile();
        }

        internal string LoadSelectedRuleset()
        {
            return _selectedRulesetEntry.Value;
        }

        /// <summary>
        /// Writes the specified ruleset to the configuration file.
        /// </summary>
        /// <param name="configName">A name under which to write the ruleset.</param>
        /// <param name="ruleset">The ruleset to write.</param>
        public static void WriteRuleset(string configName, Ruleset ruleset)
        {
            var configCategory = MelonPreferences.CreateCategory(configName);
            foreach (var rule in ruleset.Rules)
            {
                if (!(rule is IConfigWritable writableRule))
                {
                    RulesAPI.Logger.Warning($"Rule {rule.GetType().FullName} does not implement IConfigWritable. Skipping writing rule.");
                    continue;
                }

                configCategory.CreateEntry(writableRule.GetType().Name, writableRule.ToConfigString());
            }

            configCategory.SaveToFile();
        }

        /// <summary>
        /// Reads a ruleset from the configuration file.
        /// </summary>
        /// <param name="configName">The name under which the desired ruleset is written.</param>
        /// <returns>The ruleset read the configuration.</returns>
        public static Ruleset ReadRuleset(string configName)
        {
            var configCategory = MelonPreferences.CreateCategory(configName);

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

            return Ruleset.NewInstance(configName, "A custom ruleset.", rules);
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
