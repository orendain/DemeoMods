namespace RulesAPI.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using HarmonyLib;
    using MelonLoader;

    public class ConfigManager
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { IncludeFields = true, WriteIndented = true };

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

            var ruleEntries = new List<RuleConfigEntry>();
            foreach (var rule in ruleset.Rules)
            {
                try
                {
                    FindRuleAndConfigType(rule.GetType().FullName);
                    var configObject = Traverse.Create(rule).Method("GetConfigObject").GetValue();
                    var configJson = JsonSerializer.SerializeToElement(configObject, SerializerOptions);

                    var ruleEntry = new RuleConfigEntry { Rule = rule.GetType().Name, Config = configJson };
                    ruleEntries.Add(ruleEntry);
                }
                catch (Exception e)
                {
                    ConfigurationMod.Logger.Warning($"Failed to write rule entry {rule.GetType().Name} to config. Skipping that rule: {e}");
                }
            }

            var serializedRuleEntries = JsonSerializer.Serialize(ruleEntries, SerializerOptions);

            configCategory.CreateEntry("name", string.Empty).Value = ruleset.Name;
            configCategory.CreateEntry("description", string.Empty).Value = ruleset.Description;
            configCategory.CreateEntry("rules", string.Empty).Value = serializedRuleEntries;
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

            var rulesetNameEntry = configCategory.CreateEntry("name", string.Empty);
            var descriptionEntry = configCategory.CreateEntry("description", string.Empty);
            var rulesEntry = configCategory.CreateEntry("rules", string.Empty);

            var rules = new List<Rule>();
            using (var document = JsonDocument.Parse(rulesEntry.Value))
            {
                foreach (var ruleEntryJson in document.RootElement.EnumerateArray())
                {
                    try
                    {
                        var ruleEntry = ruleEntryJson.Deserialize<RuleConfigEntry>(SerializerOptions);
                        var (ruleType, configType) = FindRuleAndConfigType(ruleEntry.Rule);
                        var configObject = JsonSerializer.Deserialize(ruleEntry.Config, configType, SerializerOptions);

                        var instantiateRuleNew = InstantiateRule(ruleType, configType, configObject);
                        rules.Add(instantiateRuleNew);
                    }
                    catch (Exception e)
                    {
                        ConfigurationMod.Logger.Warning($"Failed to read rule entry from config. Skipping that rule: {e}");
                    }
                }
            }

            return Ruleset.NewInstance(rulesetNameEntry.Value, descriptionEntry.Value, rules);
        }

        private static (Type RuleType, Type ConfigType) FindRuleAndConfigType(string ruleName)
        {
            var ruleType = AccessTools.TypeByName(ruleName);
            if (ruleType == null || !typeof(Rule).IsAssignableFrom(ruleType))
            {
                throw new ArgumentException($"Could not find a rule type corresponding to name: {ruleName}");
            }

            foreach (var i in ruleType.GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigWritable<>))
                {
                    return (ruleType, i.GetGenericArguments()[0]);
                }
            }

            throw new ArgumentException($"Found rule type does not implement IConfigWritable: {ruleType.FullName}");
        }

        /// <summary>
        /// Instantiates a rule given the rule name and its configuration object.
        /// </summary>
        /// <param name="ruleType">The type of the rule to instantiate.</param>
        /// <param name="configType">The type of the rule's config object.</param>
        /// <param name="configObject">The object representing the rule's configuration.</param>
        /// <returns>An instantiated rule configured with the specified configuration.</returns>
        /// <exception cref="ArgumentException">If an appropriate rule could not be instantiated.</exception>
        private static Rule InstantiateRule(Type ruleType, Type configType, object configObject)
        {
            var constructor = AccessTools.Constructor(ruleType, new[] { configType });
            if (constructor == null)
            {
                throw new ArgumentException($"Could not find suitable constructor for rule [{ruleType}].");
            }

            try
            {
                return constructor.Invoke(new[] { configObject }) as Rule;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Failed to call rule [{ruleType}] constructor with type [{configType}]: {e}");
            }
        }

        public struct RuleConfigEntry
        {
            public string Rule;
            public JsonElement Config;
        }
    }
}
