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
        private static readonly Harmony RulesPatcher = new Harmony("com.orendain.demeomods.rulesapi.patcher");

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.rulesapi");
            ModPatcher.Patch(harmony);
        }

        public override void OnApplicationLateStart()
        {
            LoadRegisteredRules();
        }

        public static void SetSelectedRuleset(Ruleset ruleset)
        {
            if (!Registrar.Instance().IsRegistered(ruleset))
            {
                throw new ArgumentException("Ruleset must first be registered.");
            }

            ModState.SelectedRuleset = ruleset;
            Logger.Msg($"Ruleset selected: {ruleset.GetType()}");
        }

        private static void LoadRegisteredRules()
        {
            Logger.Msg($"Loading [{Registrar.Instance().RuleTypes.Count}] registered rule types.");

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
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    Logger.Error($"Failed to apply patch for rule [{ruleType}]: {e}");
                }
            }
        }

        internal static void ActivateSelectedRuleset()
        {
            // TODO(orendain): Do not automatically load the first ruleset when a game is hosted. For dev only.
            if (ModState.SelectedRuleset == null)
            {
                SetSelectedRuleset(Registrar.Instance().Rulesets.ElementAt(0));
            }

            ModState.SelectedRuleset.Activate();
        }

        internal static void DeactivateSelectedRuleset()
        {
            ModState.SelectedRuleset.Deactivate();
        }
    }
}
