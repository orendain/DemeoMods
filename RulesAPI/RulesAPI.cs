namespace RulesAPI
{
    using System;
    using System.Linq;
    using MelonLoader;

    public static class RulesAPI
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI");

        internal static Ruleset SelectedRuleset { get; private set; }

        private static bool _isRulesetActive;

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
            SelectedRuleset.Activate();
        }

        internal static void DeactivateSelectedRuleset()
        {
            if (!_isRulesetActive)
            {
                return;
            }

            SelectedRuleset.Deactivate();
        }

        internal static void TriggerPreGameCreated()
        {
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    rule.PreGameCreated();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Rollback activation.
                    Logger.Warning($"Failed to successfully call PreGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerPostGameCreated()
        {
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    rule.PostGameCreated();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Rollback activation.
                    Logger.Warning($"Failed to successfully call PostGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }
    }
}
