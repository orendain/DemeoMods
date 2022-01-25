﻿namespace RulesAPI
{
    using System;
    using System.Linq;
    using HarmonyLib;
    using MelonLoader;

    internal class RulesAPIMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI");
        private static readonly ModState ModState = ModState.NewInstance();
        private static readonly Harmony RulesPatcher = new Harmony("com.orendain.demeomods.rules.api.patcher");

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.rules.api");
            ModPatcher.Patch(harmony);
        }

        public override void OnApplicationLateStart()
        {
            LoadRegisteredRules();
            SetSelectedRuleSet(Registrar.Instance().RuleSets.ElementAt(0));
        }

        public void SetSelectedRuleSet(RuleSet ruleSet)
        {
            if (!Registrar.Instance().IsRegistered(ruleSet))
            {
                throw new ArgumentException("Rule set must first be registered.");
            }

            ModState.SelectedRuleSet = ruleSet;
            Logger.Msg($"Rule selected: {ruleSet.GetType()}");
        }

        private static void LoadRegisteredRules()
        {
            Logger.Msg($"Loading [{Registrar.Instance().RuleTypes.Count}] registered rules types.");

            foreach (var ruleType in Registrar.Instance().RuleTypes)
            {
                Logger.Msg($"Loading rule type: {ruleType}");
                var patchMethod = AccessTools.Method(ruleType, "OnPatch");
                if (patchMethod == null)
                {
                    Logger.Warning($"Did not find suitable 'OnPatch' method for rule [{ruleType}]. No patching is done for that rule.");
                    continue;
                }

                try
                {
                    patchMethod.Invoke(null, new object[] { RulesPatcher });
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/ruleSets that fail to patch/load.
                    Logger.Error($"Failed to apply patch for rule [{ruleType}]: {e}");
                }
            }
        }

        internal static void ActivateSelectedRuleSet()
        {
            if (ModState.SelectedRuleSet == null)
            {
                return;
            }

            ModState.SelectedRuleSet.Activate();
        }

        internal static void DeactivateSelectedRuleSet()
        {
            ModState.SelectedRuleSet.Deactivate();
        }
    }
}
