namespace RulesAPI
{
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
    }
}
