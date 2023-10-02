namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal static class RulesetImporter
    {
        /// <summary>
        /// Returns a list of all files in the specified directory that may qualify as ruleset files.
        /// </summary>
        /// <param name="directory">Path to the directory to search for rulesets.</param>
        /// <returns>A list of full file paths to all ruleset files found.</returns>
        internal static List<string> ListRulesets(string directory)
        {
            return Directory.EnumerateFiles(directory, "*.json").ToList();
        }

        /// <summary>
        /// Write the specified ruleset to the specified directory.
        /// </summary>
        /// <param name="ruleset">The ruleset to export.</param>
        /// <param name="directory">The path of the directory to export to.</param>
        /// <returns>The path of the file that the ruleset was written to.</returns>
        internal static string WriteToDirectory(Ruleset ruleset, string directory)
        {
            SetDefaultSerializationSettings();

            if (string.IsNullOrEmpty(ruleset.Name))
            {
                throw new ArgumentException("Ruleset name must not be empty.");
            }

            var ruleEntries = new List<RuleConfigEntry>();
            foreach (var rule in ruleset.Rules)
            {
                try
                {
                    FindRuleAndConfigType(rule.GetType().Name);
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
                    HouseRulesConfigurationBase.LogWarning($"Failed to write rule entry {rule.GetType().Name} to config. Skipping that rule: {e}");
                }
            }

            var rulesetConfig = new RulesetConfig
            {
                Name = ruleset.Name,
                Description = ruleset.Description,
                Longdesc = ruleset.Longdesc,
                Rules = ruleEntries,
            };
            var serializedRuleset = JsonConvert.SerializeObject(rulesetConfig);
            var rulesetFilename = SanitizeRulesetFilename(ruleset.Name);
            var rulesetFilePath = Path.Combine(directory, $"{rulesetFilename}.json");
            File.WriteAllText(rulesetFilePath, serializedRuleset);

            HouseRulesConfigurationBase.LogInfo($"Successfully exported ruleset to: {rulesetFilePath}");
            return rulesetFilePath;
        }

        /// <summary>
        /// Reads a ruleset from the specified full file name.
        /// </summary>
        /// <remarks>
        /// Tolerating failures via <c>tolerateFailures</c> continues importing the ruleset even
        /// when individual rules fail to import by skipping over those that are erroneous.
        /// </remarks>
        /// <param name="fileName">The full file name of the JSON file to load as a ruleset.</param>
        /// <param name="tolerateFailures">Whether or not to tolerate partial failures.</param>
        /// <returns>The imported ruleset.</returns>
        internal static Ruleset Read(string fileName, bool tolerateFailures)
        {
            SetDefaultSerializationSettings();

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

                    HouseRulesConfigurationBase.LogWarning($"Failed to read rule entry [{ruleConfigEntry.Rule}] from config. Tolerating failures by skipping that rule: {e}");
                }
            }

            HouseRulesConfigurationBase.LogDebug($"Successfully imported ruleset from: {fileName}");
            return Ruleset.NewInstance(rulesetConfig.Name, rulesetConfig.Description, rulesetConfig.Longdesc, rules);
        }

        private static (Type RuleType, Type ConfigType) FindRuleAndConfigType(string ruleName)
        {
            if (!TryFindRuleType(ruleName, out var ruleType))
            {
                throw new ArgumentException($"Could not find a registered rule represented by name: {ruleName}");
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
        /// Finds the registered rule type represented by the specified rule name, returning false
        /// if no matching rule has been registered.
        /// </summary>
        private static bool TryFindRuleType(string ruleName, out Type ruleType)
        {
            foreach (var r in HR.Rulebook.RuleTypes)
            {
                if (ExpandRuleName(ruleName).Equals(r.Name) || ruleName.Equals(r.Name))
                {
                    ruleType = r;
                    return true;
                }
            }

            ruleType = null;
            return false;
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
        /// Returns a modified version of the specified string, sanitized for writing to the filesystem.
        /// </summary>
        private static string SanitizeRulesetFilename(string rulesetName)
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
            public string Longdesc;
            public List<RuleConfigEntry> Rules;
        }

        private struct RuleConfigEntry
        {
            public string Rule;
            public JToken Config;
        }
    }
}
