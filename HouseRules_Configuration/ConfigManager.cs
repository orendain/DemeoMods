namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using HarmonyLib;
    using HouseRules.Types;
    using MelonLoader;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ConfigManager
    {
        internal static readonly string RulesetDirectory = Path.Combine(MelonUtils.UserDataDirectory, "HouseRules");

        private readonly MelonPreferences_Category _configCategory;
        private readonly MelonPreferences_Entry<string> _defaultRulesetEntry;
        private readonly MelonPreferences_Entry<bool> _loadRulesetsFromConfigEntry;

        internal static ConfigManager NewInstance()
        {
            return new ConfigManager();
        }

        private ConfigManager()
        {
            _configCategory = MelonPreferences.CreateCategory("HouseRules");
            _defaultRulesetEntry = _configCategory.CreateEntry("defaultRuleset", string.Empty);
            _loadRulesetsFromConfigEntry = _configCategory.CreateEntry("loadRulesetsFromConfig", true);
            Directory.CreateDirectory(RulesetDirectory);
            SetDefaultSerializationSettings();
        }

        internal void SetDefaultRuleset(string rulesetName)
        {
            _defaultRulesetEntry.Value = rulesetName;
        }

        internal void SetLoadRulesetsFromConfig(bool loadFromConfig)
        {
            _loadRulesetsFromConfigEntry.Value = loadFromConfig;
        }

        internal string GetDefaultRuleset()
        {
            return _defaultRulesetEntry.Value;
        }

        internal bool GetLoadRulesetsFromConfig()
        {
            return _loadRulesetsFromConfigEntry.Value;
        }

        internal void Save()
        {
            _configCategory.SaveToFile();
        }

        /// <summary>
        /// Gets a list of all ruleset files founds.
        /// </summary>
        internal List<string> RulesetFiles => Directory.EnumerateFiles(RulesetDirectory, "*.json").ToList();

        /// <summary>
        /// Exports the specified ruleset by writing it to a file in the default ruleset directory.
        /// </summary>
        /// <param name="ruleset">The ruleset to export.</param>
        /// <returns>The path of the file that the ruleset was written to.</returns>
        internal string ExportRuleset(Ruleset ruleset)
        {
            return ExportRuleset(ruleset, RulesetDirectory);
        }

        /// <summary>
        /// Exports the specified ruleset by writing it to a file in the specified directory.
        /// </summary>
        /// <param name="ruleset">The ruleset to export.</param>
        /// <param name="directory">The path of the directory to export to.</param>
        /// <returns>The path of the file that the ruleset was written to.</returns>
        internal string ExportRuleset(Ruleset ruleset, string directory)
        {
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
            var rulesetFilename = SanitizeRulesetFilename(ruleset.Name);
            var rulesetFilePath = Path.Combine(directory, $"{rulesetFilename}.json");
            File.WriteAllText(rulesetFilePath, serializedRuleset);

            ConfigurationMod.Logger.Msg($"Successfully exported ruleset to: {rulesetFilePath}");
            return rulesetFilePath;
        }

        /// <summary>
        /// Imports a ruleset by full file name.
        /// </summary>
        /// <remarks>
        /// Tolerating failures via <c>tolerateFailures</c> continues importing the ruleset even
        /// when individual rules fail to import by skipping over those that are erroneous.
        /// </remarks>
        /// <param name="fileName">The full file name of the JSON file to load as a ruleset.</param>
        /// <param name="tolerateFailures">Whether or not to tolerate partial failures.</param>
        /// <returns>The imported ruleset.</returns>
        internal static Ruleset ImportRuleset(string fileName, bool tolerateFailures)
        {
            var rulesetJson = File.ReadAllText(fileName);
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
                    if (!tolerateFailures)
                    {
                        throw new InvalidOperationException($"Failed to read rule entry [{ruleConfigEntry.Rule}] of ruleset [{rulesetConfig.Name}].", e);
                    }

                    ConfigurationMod.Logger.Warning($"Failed to read rule entry [{ruleConfigEntry.Rule}] from config. Tolerating failures by skipping that rule: {e}");
                }
            }

            ConfigurationMod.Logger.Msg($"Successfully imported ruleset from: {fileName}");
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

        private static void SetDefaultSerializationSettings()
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

        internal static string SanitizeRulesetFilename(string rulesetName)
        {
            var invalids = Path.GetInvalidFileNameChars();
            return string.Join(string.Empty, rulesetName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
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
