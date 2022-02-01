using System.Collections.Generic;
using MelonLoader.TinyJSON;

namespace RulesAPI
{
    using System;
    using HarmonyLib;
    using MelonLoader;

    internal class RulesAPIMod : MelonMod
    {
        private static readonly Harmony RulesPatcher = new Harmony("com.orendain.demeomods.rulesapi.patcher");

        private MelonPreferences_Category _configCategory;
        private MelonPreferences_Entry<string> _rulesetConfig;

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.rulesapi");
            ModPatcher.Patch(harmony);
        }

        public override void OnApplicationLateStart()
        {
            LoadRegisteredRules();
            InitializeConfig();
        }

        public override void OnApplicationQuit()
        {
            _rulesetConfig.Value = RulesAPI.SelectedRuleset != null ? RulesAPI.SelectedRuleset.Name : string.Empty;
            _configCategory.SaveToFile();
        }

        private void InitializeConfig()
        {
            _configCategory = MelonPreferences.CreateCategory("RulesAPI");
            _rulesetConfig = _configCategory.CreateEntry("ruleset", string.Empty);

            temp();

            if (string.IsNullOrEmpty(_rulesetConfig.Value))
            {
                return;
            }

            try
            {
                RulesAPI.SelectRuleset(_rulesetConfig.Value);
            }
            catch (ArgumentException e)
            {
                RulesAPI.Logger.Warning($"Failed to select ruleset [{_rulesetConfig.Value}] from config: {e}");
            }
        }

        private void temp()
        {

            // var entry = _configCategory.CreateEntry("myEntry", heroesCards);


            var tete = new List<List<string>>()
            {
                new List<string>() {"SongOfRecovery", "true"},
                new List<string>() {"234234", "true"},
                new List<string>() {"2333zzz", "true"},
                new List<string>() {"SongOfR234234ecovery", "true"},
                new List<string>() {"ddd", "true"},
            };

            var dump1 = JSON.Dump(tete);
            var dump2 = JSON.Dump(tete, EncodeOptions.PrettyPrint);
            // var one = _configCategory.CreateEntry("One1", default_value: tete);
            var two = _configCategory.CreateEntry("One2", default_value: dump1);
            _configCategory.CreateEntry("One3", default_value: dump2);


            var tete3 = new Dictionary<string, List<string>>()
            {
                {"Sup", new List<string>() {"SongOfRecovery", "true"}},
                    {"Su4p", new List<string>() {"234234", "true"}},
                {"Su3p", new List<string>() {"2333zzz", "true"}},
                    {"Su2", new List<string>() {"SongOfR234234ecovery", "true"}},
                    {"Sup1", new List<string>() {"ddd", "true"}},
            };

            var dump3 = JSON.Dump(tete3);
            var dump4 = JSON.Dump(tete3, EncodeOptions.PrettyPrint);
            // var three = _configCategory.CreateEntry("Two1", default_value: tete3);
            var four = _configCategory.CreateEntry("Two2", default_value: dump3);
            var sdfsdfsdf = _configCategory.CreateEntry("Two3", default_value: dump4);

            JSON.MakeInto(JSON.Load(sdfsdfsdf.Value), out Dictionary<string, List<string>> outie);

            RulesAPI.Logger.Msg(outie);


            foreach (var keyValuePair in outie)
            {
                RulesAPI.Logger.Msg($"{keyValuePair.Key}, {keyValuePair.Value}");
            }
        }

        private static void LoadRegisteredRules()
        {
            RulesAPI.Logger.Msg($"Loading [{Registrar.Instance().RuleTypes.Count}] registered rule types.");

            foreach (var ruleType in Registrar.Instance().RuleTypes)
            {
                RulesAPI.Logger.Msg($"Loading rule type: {ruleType}");
                var patchMethod = AccessTools.Method(ruleType, "OnPatch");
                if (patchMethod == null)
                {
                    RulesAPI.Logger.Warning($"Did not find suitable 'OnPatch' method for rule [{ruleType}]. No patching is done for that rule.");
                    continue;
                }

                try
                {
                    patchMethod.Invoke(null, new object[] { RulesPatcher });
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    RulesAPI.Logger.Error($"Failed to apply patch for rule [{ruleType}]: {e}");
                }
            }
        }
    }
}
