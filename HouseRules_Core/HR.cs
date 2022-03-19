namespace HouseRules
{
    using System;
    using System.Linq;
    using System.Text;
    using Boardgame;
    using DataKeys;
    using HouseRules.Types;
    using MelonLoader;

    public static class HR
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Core");
        private const float WelcomeMessageDurationSeconds = 30f;

        public static readonly Rulebook Rulebook = Rulebook.NewInstance();

        public static Ruleset SelectedRuleset { get; private set; } = Ruleset.None;

        internal static bool IsRulesetActive { get; private set; }

        public static void SelectRuleset(string ruleset)
        {
            if (IsRulesetActive)
            {
                throw new InvalidOperationException("May not select a new ruleset while one is currently active.");
            }

            if (Ruleset.None.Name.Equals(ruleset, StringComparison.OrdinalIgnoreCase))
            {
                SelectedRuleset = Ruleset.None;
                Logger.Msg("Cleared selected ruleset.");
                return;
            }

            if (!Rulebook.IsRulesetRegistered(ruleset))
            {
                throw new ArgumentException("Ruleset must first be registered.");
            }

            SelectedRuleset = Rulebook.Rulesets.First(r => string.Equals(r.Name, ruleset, StringComparison.OrdinalIgnoreCase));

            Logger.Msg($"Selected ruleset: {SelectedRuleset.Name}");
        }

        internal static void TriggerActivateRuleset(GameContext gameContext, GameHub.GameMode gameMode)
        {
            if (IsRulesetActive)
            {
                Logger.Warning("Ruleset activation was triggered whilst a ruleset was already activated. This should not happen. Please report this to HouseRules developers.");
                return;
            }

            if (SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (gameMode == GameHub.GameMode.Multiplayer && !SelectedRuleset.IsSafeForMultiplayer)
            {
                Logger.Warning($"The selected ruleset [{SelectedRuleset.Name}] is not safe for multiplayer games. Skipping activation.");
                return;
            }

            IsRulesetActive = true;

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
            if (!IsRulesetActive)
            {
                return;
            }

            IsRulesetActive = false;

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
            if (SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
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
            if (SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
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
            if (SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                GameUI.ShowCameraMessage(BuildNotSafeForMultiplayerMessage(), WelcomeMessageDurationSeconds);
                return;
            }

            GameUI.ShowCameraMessage(BuildRulesetActiveMessage(), WelcomeMessageDurationSeconds);
        }

        private static string BuildNotSafeForMultiplayerMessage()
        {
            return new StringBuilder()
                .AppendLine("Attention:")
                .AppendLine("The HouseRules ruleset you selected is not safe for multiplayer games, and was not activated.")
                .ToString();
        }

        private static string BuildRulesetActiveMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Welcome to a game using HouseRules!");
            sb.AppendLine();
            sb.AppendLine($"{SelectedRuleset.Name}:");
            sb.AppendLine(SelectedRuleset.Description);
            sb.AppendLine();
            sb.AppendLine("Rules:");

            for (var i = 0; i < SelectedRuleset.Rules.Count; i++)
            {
                var description = SelectedRuleset.Rules[i].Description;
                sb.AppendLine($"{i + 1}. {description}");
            }

            return sb.ToString();
        }
    }
}
