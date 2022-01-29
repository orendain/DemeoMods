namespace RulesAPI
{
    using System;
    using System.Linq;
    using MelonLoader;

    public static class RulesAPI
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI");

        private static bool _isRulesetActive;

        internal static Ruleset SelectedRuleset { get; private set; }

        public static void SelectRuleset(string ruleset)
        {
            try
            {
                SelectedRuleset = Registrar.Instance().Rulesets
                    .Single(r => string.Equals(r.Name, ruleset, StringComparison.OrdinalIgnoreCase));
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException("Ruleset must first be registered.", e);
            }

            Logger.Msg($"Selected ruleset: {SelectedRuleset.Name}");
        }

        internal static void ActivateSelectedRuleset()
        {
            if (_isRulesetActive)
            {
                Logger.Warning("Ruleset activation was triggered while ruleset was already activated. This should not happen. Please report this to RulesAPI developers.");
                return;
            }

            if (SelectedRuleset == null)
            {
                return;
            }

            _isRulesetActive = true;

            Logger.Msg($"Activating ruleset: {SelectedRuleset.Name} (with {SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Activating rule type: {rule.GetType()}");
                    rule.OnActivate();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to activate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void DeactivateSelectedRuleset()
        {
            if (!_isRulesetActive)
            {
                return;
            }

            Logger.Msg($"Deactivating ruleset: {SelectedRuleset.Name} (with {SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Deactivating rule type: {rule.GetType()}");
                    rule.OnDeactivate();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to deactivate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerPreGameCreated()
        {
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Calling OnPreGameCreated for rule type: {rule.GetType()}");
                    rule.PreGameCreated();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to successfully call OnPreGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerPostGameCreated()
        {
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Calling OnPostGameCreated for rule type: {rule.GetType()}");
                    rule.PostGameCreated();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to successfully call OnPostGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }
    }
}
