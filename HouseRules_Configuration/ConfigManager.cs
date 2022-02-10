namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using HarmonyLib;
    using HouseRules.Types;
    using MelonLoader;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ConfigManager
    {
        private readonly MelonPreferences_Category _configCategory;
        private readonly MelonPreferences_Entry<string> _rulesetEntry;
        private readonly MelonPreferences_Entry<bool> _loadFromConfigEntry;

        internal static ConfigManager NewInstance()
        {
            return new ConfigManager();
        }

        private ConfigManager()
        {
            _configCategory = MelonPreferences.CreateCategory("HouseRules");
            _rulesetEntry = _configCategory.CreateEntry("ruleset", string.Empty);
            _loadFromConfigEntry = _configCategory.CreateEntry("loadFromConfig", false);
        }

        internal void SetRuleset(string rulesetName)
        {
            _rulesetEntry.Value = rulesetName;
        }

        internal void SetLoadFromConfig(bool loadFromConfig)
        {
            _loadFromConfigEntry.Value = loadFromConfig;
        }

        internal string GetRuleset()
        {
            return _rulesetEntry.Value;
        }

        internal bool GetLoadFromConfig()
        {
            return _loadFromConfigEntry.Value;
        }

        internal void Save()
        {
            _configCategory.SaveToFile();
        }

        /// <summary>
        /// Exports the specified ruleset by writing it to a file.
        /// </summary>
        /// <param name="ruleset">The ruleset to export.</param>
        /// <returns>The path of the file that the ruleset was written to.</returns>
        public static string ExportRuleset(Ruleset ruleset)
        {
            ConfigureDefaultSerializationSettings();

            if (string.IsNullOrEmpty(ruleset.Name))
            {
                throw new ArgumentException("Ruleset name must not be empty.");
            }

            var ruleEntries = new List<RuleConfigEntry>();
            foreach (var rule in ruleset.Rules)
            {
                try
                {
                    FindRuleAndConfigType(rule.GetType().FullName);
                    var configObject = Traverse.Create(rule).Method("GetConfigObject").GetValue();
                    var configJson = JToken.FromObject(configObject);
                    var ruleEntry = new RuleConfigEntry
                    {
                        Rule = ShortenRuleName(rule.GetType().Name),
                        Config = configJson,
                    };
                    ruleEntries.Add(ruleEntry);
                }
                catch (Exception e)
                {
                    ConfigurationMod.Logger.Warning($"Failed to write rule entry {rule.GetType().Name} to config. Skipping that rule: {e}");
                }
            }

            var rulesetConfig = new RulesetConfig
            {
                Name = ruleset.Name,
                Description = ruleset.Description,
                Rules = ruleEntries,
            };
            var serializedRuleset = JsonConvert.SerializeObject(rulesetConfig);

            var houseRulesDataDir = Path.Combine(MelonUtils.UserDataDirectory, "HouseRules");
            var rulesetFilePath = Path.Combine(houseRulesDataDir, $"{ruleset.Name}.json");
            Directory.CreateDirectory(houseRulesDataDir);
            File.WriteAllText(rulesetFilePath, serializedRuleset);

            ConfigurationMod.Logger.Msg($"Successfully exported ruleset to: {rulesetFilePath}");

            return rulesetFilePath;
        }

        /// <summary>
        /// Imports a ruleset by name.
        /// </summary>
        /// <param name="rulesetName">The name of the ruleset to import, saved as a JSON file at an internally-resolved location.</param>
        /// <returns>The imported ruleset.</returns>
        internal static Ruleset ImportRuleset(string rulesetName)
        {
            ConfigureDefaultSerializationSettings();

            var rulesetFilePath = Path.Combine(MelonUtils.UserDataDirectory, "HouseRules", $"{rulesetName}.json");
            var rulesetJson = File.ReadAllText(rulesetFilePath);
            var rulesetConfig = JsonConvert.DeserializeObject<RulesetConfig>(rulesetJson);

            var rules = new List<Rule>();
            foreach (var ruleConfigEntry in rulesetConfig.Rules)
            {
                try
                {
                    var (ruleType, configType) = FindRuleAndConfigType(ruleConfigEntry.Rule);
                    var configObject = ruleConfigEntry.Config.ToObject(configType);
                    var instantiateRuleNew = InstantiateRule(ruleType, configType, configObject);
                    rules.Add(instantiateRuleNew);
                }
                catch (Exception e)
                {
                    ConfigurationMod.Logger.Warning($"Failed to read rule entry from config. Skipping that rule: {e}");
                }
            }

            ConfigurationMod.Logger.Msg($"Successfully imported ruleset from: {rulesetFilePath}");
            return Ruleset.NewInstance(rulesetConfig.Name, rulesetConfig.Description, rules);
        }

        private static (Type RuleType, Type ConfigType) FindRuleAndConfigType(string ruleName)
        {
            var ruleType = AccessTools.TypeByName(ruleName) ?? AccessTools.TypeByName(ExpandRuleName(ruleName));

            if (ruleType == null)
            {
                throw new ArgumentException($"Could not find a rule type represented by the name: {ruleName}");
            }

            if (!typeof(Rule).IsAssignableFrom(ruleType))
            {
                throw new ArgumentException($"Failed to recognize the type found as representing a rule: {ruleType.FullName}");
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

        private static void ConfigureDefaultSerializationSettings()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() },
            };
        }

        /// <summary>
        /// Shorts the specified rule name by removing any rule type suffix.
        /// </summary>
        private static string ShortenRuleName(string ruleName)
        {
            return ruleName.EndsWith("Rule", StringComparison.OrdinalIgnoreCase)
                ? ruleName.Substring(0, ruleName.Length - 4)
                : ruleName;
        }

        /// <summary>
        /// Expands the specified rule name by adding a rule suffix.
        /// </summary>
        private static string ExpandRuleName(string ruleName)
        {
            return $"{ruleName}Rule";
        }

        private struct RulesetConfig
        {
            public string Name;
            public string Description;
            public List<RuleConfigEntry> Rules;
        }

        private struct RuleConfigEntry
        {
            public string Rule;
            public JToken Config;
        }
    }
}
