namespace HouseRules
{
    using System;
    using System.Linq;
    using System.Text;
    using Boardgame;
    using MelonLoader;

    public static class HouseRules
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Core");
        private const float WelcomeMessageDurationSeconds = 30f;
        private static bool _isRulesetActive;

        public static Ruleset SelectedRuleset { get; private set; }

        public static void SelectRuleset(string ruleset)
        {
            if (_isRulesetActive)
            {
                throw new InvalidOperationException("May not select a new ruleset while one is currently active.");
            }

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

        internal static void TriggerActivateRuleset(GameContext gameContext)
        {
            if (_isRulesetActive)
            {
                Logger.Warning("Ruleset activation was triggered while ruleset was already activated. This should not happen. Please report this to HouseRules developers.");
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
                    rule.OnActivate(gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to activate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerDeactivateRuleset(GameContext gameContext)
        {
            if (!_isRulesetActive)
            {
                return;
            }

            _isRulesetActive = false;

            Logger.Msg($"Deactivating ruleset: {SelectedRuleset.Name} (with {SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Deactivating rule type: {rule.GetType()}");
                    rule.OnDeactivate(gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to deactivate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerPreGameCreated(GameContext gameContext)
        {
            if (SelectedRuleset == null)
            {
                return;
            }

            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Calling OnPreGameCreated for rule type: {rule.GetType()}");
                    rule.OnPreGameCreated(gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to successfully call OnPreGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerPostGameCreated(GameContext gameContext)
        {
            if (SelectedRuleset == null)
            {
                return;
            }

            foreach (var rule in SelectedRuleset.Rules)
            {
                try
                {
                    Logger.Msg($"Calling OnPostGameCreated for rule type: {rule.GetType()}");
                    rule.OnPostGameCreated(gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    Logger.Warning($"Failed to successfully call OnPostGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal static void TriggerWelcomeMessage()
        {
            if (!_isRulesetActive)
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Welcome to a game using a custom ruleset!");
            sb.AppendLine();
            sb.AppendFormat("{0}: {1}\n", SelectedRuleset.Name, SelectedRuleset.Description);
            sb.AppendLine();
            sb.AppendLine("Rules:");
            foreach (var rule in SelectedRuleset.Rules)
            {
                sb.AppendLine(rule.Description);
            }

            GameUI.ShowCameraMessage(sb.ToString(), WelcomeMessageDurationSeconds);
        }
    }
}
